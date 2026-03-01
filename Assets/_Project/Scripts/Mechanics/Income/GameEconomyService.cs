using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Mechanics.Shops;
using Core.Scripts.Debugging;
using Core.Scripts.Services.UpdateService;
using Core.Storage.Bank;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.Config;
using Mechanics.DayNight;
using Mechanics.GameSave;
using Zenject;

namespace Mechanics.Income
{
    public class GameEconomyService : IGameEconomyService, IUpdatable
    {
        private readonly Dictionary<string, float> _companiesIncomes = new();
        private readonly Dictionary<string, float> _charactersIncomes = new();

        private IDataBank<float> _cleanBank;
        private IDataBank<float> _dirtyBank;
        private IDataBank<float> _reputationBank;
        private IDataBank<float> _heatBank;
        private ICharacterData _myCharacter;

        private readonly ICompaniesProvider _companiesProvider;
        private readonly ICharactersProvider _charactersProvider;
        private readonly IBanksProvidersProvider _banksProvidersProvider;
        private readonly IUpdateService _updateService;
        private readonly ICompaniesService _companiesService;
        private readonly ICharacterCompanyListener _characterCompanyListener;
        private readonly IGameTimeService _gameTimeService;
        private readonly GameEconomyConfig _economyConfig;
        private readonly IGameSaveService _gameSaveService;

        public event Action OnMyPlayerIncomeChanged;
        public event Action<float> OnPlayerBalanceChanged;
        public event Action<float> OnCleanBalanceChanged;
        public event Action<float> OnDirtyBalanceChanged;
        public event Action<float> OnReputationChanged;
        public event Action<float> OnHeatChanged;
        public UpdateType UpdateType => UpdateType.Update;

        [Inject]
        public GameEconomyService(
            ICompaniesProvider companiesProvider,
            ICharactersProvider charactersProvider,
            IBanksProvidersProvider banksProvidersProvider,
            IUpdateService updateService,
            ICompaniesService companiesService,
            ICharacterCompanyListener characterCompanyListener,
            IGameTimeService gameTimeService,
            GameEconomyConfig economyConfig,
            IGameSaveService gameSaveService)
        {
            _companiesProvider = companiesProvider;
            _charactersProvider = charactersProvider;
            _banksProvidersProvider = banksProvidersProvider;
            _updateService = updateService;
            _companiesService = companiesService;
            _characterCompanyListener = characterCompanyListener;
            _gameTimeService = gameTimeService;
            _economyConfig = economyConfig;
            _gameSaveService = gameSaveService;
        }

        public async Task Init()
        {
            foreach (var characterData in _charactersProvider.GetAllCharacter())
            {
                _charactersIncomes.TryAdd(characterData.Key,CalculateCharacterIncomePerMonth(characterData));
            }

            _cleanBank = _banksProvidersProvider.GetFloatBankProvider().Get(BankId.CleanMoney);
            _dirtyBank = _banksProvidersProvider.GetFloatBankProvider().Get(BankId.DirtyMoney);
            _reputationBank = _banksProvidersProvider.GetFloatBankProvider().Get(BankId.Reputation);
            _heatBank = _banksProvidersProvider.GetFloatBankProvider().Get(BankId.Heat);
            _myCharacter = _charactersProvider.GetMyCharacter();

            if (!_gameSaveService.IsGameLoaded() && _economyConfig.StartMoney > 0f)
                AddCleanMoney(_economyConfig.StartMoney);

            _characterCompanyListener.OnMyCharacterCompanyDataChanged += CharacterCompanyListenerOnOnMyCharacterCompanyDataChanged;

            OnMyPlayerIncomeChanged?.Invoke();

            _updateService.Add(this);
        }

        private void CharacterCompanyListenerOnOnMyCharacterCompanyDataChanged(ICompanyData data) => CalculateCharacterIncomePerMonth(_charactersProvider.GetCharacterByKey(data.Owner));

        public float GetCharacterIncomePerMonth(ICharacterData characterData) 
            => _charactersIncomes[characterData.Key];

        public void Spend(float value) => SpendClean(value);

        public bool CanSpend(float value) => CanSpendClean(value);

        public void SpendClean(float value)
        {
            _cleanBank.Spend(value);
            OnPlayerBalanceChanged?.Invoke(_cleanBank.GetValue());
            OnCleanBalanceChanged?.Invoke(_cleanBank.GetValue());
        }

        public bool CanSpendClean(float value) => _cleanBank.CanSpend(value);

        public void SpendDirty(float value)
        {
            _dirtyBank.Spend(value);
            OnDirtyBalanceChanged?.Invoke(_dirtyBank.GetValue());
        }

        public bool CanSpendDirty(float value) => _dirtyBank.CanSpend(value);

        public void LaunderDirtyToClean(float amount)
        {
            if (!_dirtyBank.CanSpend(amount)) return;
            _dirtyBank.Spend(amount);
            _cleanBank.Add(amount);
            OnPlayerBalanceChanged?.Invoke(_cleanBank.GetValue());
            OnCleanBalanceChanged?.Invoke(_cleanBank.GetValue());
            OnDirtyBalanceChanged?.Invoke(_dirtyBank.GetValue());
        }

        public bool CanLaunder(float amount) => _dirtyBank.CanSpend(amount);

        public float GetCleanBalance() => _cleanBank.GetValue();

        public float GetDirtyBalance() => _dirtyBank.GetValue();

        public void AddCleanMoney(float amount)
        {
            if (amount <= 0) return;
            _cleanBank.Add(amount);
            OnPlayerBalanceChanged?.Invoke(_cleanBank.GetValue());
            OnCleanBalanceChanged?.Invoke(_cleanBank.GetValue());
        }

        public void AddDirtyMoney(float amount)
        {
            if (amount <= 0) return;
            _dirtyBank.Add(amount);
            OnDirtyBalanceChanged?.Invoke(_dirtyBank.GetValue());
        }

        public void AddReputation(float amount)
        {
            if (amount <= 0) return;
            _reputationBank.Add(amount);
            OnReputationChanged?.Invoke(_reputationBank.GetValue());
        }

        public void AddHeat(float amount)
        {
            if (amount <= 0) return;
            _heatBank.Add(amount);
            OnHeatChanged?.Invoke(_heatBank.GetValue());
        }

        public float GetReputation() => _reputationBank.GetValue();
        public float GetHeat() => _heatBank.GetValue();

        public float GetCompanyIncomePerMonth(string companyKey)
        {
            if (_companiesIncomes.TryGetValue(companyKey, out var result))
            {
                return result;
            }
            else
            {
                CalculateCompanyIncomePerMonth(companyKey);
                return _companiesIncomes[companyKey];
            }
        }

        public void ManualUpdateIncome(string characterKey)
        {
            CalculateCharacterIncomePerMonth(_charactersProvider.GetCharacterByKey(characterKey));
        }

        private void CalculateCompanyIncomePerMonth(string companyKey)
        {
            var company = _companiesProvider.GetCompanyByKey(companyKey);
            var result = -_companiesService.GetCompanyEmployeesSalary(companyKey);
            _companiesIncomes.TryAdd(companyKey, result);
        }

        private float CalculateCharacterIncomePerMonth(ICharacterData characterData)
        {
            if (characterData == null)
                return 0f;
            var result = -_companiesService.GetCompanyEmployeesSalary(characterData.CompanyKey);

            if (_charactersIncomes.ContainsKey(characterData.Key))
                _charactersIncomes[characterData.Key] = result;
            else
                _charactersIncomes.TryAdd(characterData.Key, result);

            if (characterData == _myCharacter)
                OnMyPlayerIncomeChanged?.Invoke();

            return result;
        }

        private float GetIncomePerTick(float tick, ICharacterData characterData)
        {
            if (characterData == null)
                return 0f;
            _charactersIncomes.TryGetValue(characterData.Key, out var incomePerMonth);
            float result = ConvertMonthValueIntoTick(incomePerMonth) * tick;
            
            return result;
        }

        private float ConvertMonthValueIntoTick(float incomePerMonth) 
            => incomePerMonth / _economyConfig.TicksPerMonth;

        public void Tick(float tickTime)
        {
            var addValue = GetIncomePerTick(tickTime, _myCharacter);
            if (addValue < 0)
            {
                _cleanBank.Add(addValue);
                OnPlayerBalanceChanged?.Invoke(_cleanBank.GetValue());
                OnCleanBalanceChanged?.Invoke(_cleanBank.GetValue());
                Debugging.Log(this, $"Salary paid from clean {addValue}");
                return;
            }

            if (_gameTimeService.CurrentPhase == DayNightPhase.Day)
            {
                _cleanBank.Add(addValue);
                OnPlayerBalanceChanged?.Invoke(_cleanBank.GetValue());
                OnCleanBalanceChanged?.Invoke(_cleanBank.GetValue());
                Debugging.Log(this, $"Added clean income to player {addValue}");
            }
            else
            {
                _dirtyBank.Add(addValue);
                OnDirtyBalanceChanged?.Invoke(_dirtyBank.GetValue());
                Debugging.Log(this, $"Added dirty income to player {addValue}");
            }
        }
    }
}