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
using Mechanics.DayNight;
using Mechanics.Product;
using Mechanics.Product.Provider;
using Zenject;

namespace Mechanics.Income
{
    public class GameEconomyService : IGameEconomyService, IUpdatable
    {
        private readonly Dictionary<string, float> _companiesIncomes = new();
        private readonly Dictionary<string, float> _productsIncomes = new();
        private readonly Dictionary<string, float> _charactersIncomes = new();

        private IDataBank<float> _cleanBank;
        private IDataBank<float> _dirtyBank;
        private ICharacterData _myCharacter;

        private ICompaniesProvider _companiesProvider;
        private ICharactersProvider _charactersProvider;
        private IBanksProvidersProvider _banksProvidersProvider;
        
        private IUpdateService _updateService;
        private ICompaniesService _companiesService;
        private ICharacterCompanyListener _characterCompanyListener;
        private IProductsProvider _productsProvider;
        private IGameTimeService _gameTimeService;

        public event Action OnMyPlayerIncomeChanged;

        public event Action<float> OnPlayerBalanceChanged;

        public event Action<float> OnCleanBalanceChanged;

        public event Action<float> OnDirtyBalanceChanged;

        public UpdateType UpdateType => UpdateType.Update;


        [Inject]
        private void Construct(ICompaniesProvider companiesProvider, ICharactersProvider charactersProvider,
            IBanksProvidersProvider banksProvidersProvider, IUpdateService updateService, ICompaniesService companiesService,
            ICharacterCompanyListener characterCompanyListener, IProductsProvider productsProvider,
            IGameTimeService gameTimeService)
        {
            _gameTimeService = gameTimeService;
            _productsProvider = productsProvider;
            _characterCompanyListener = characterCompanyListener;
            _companiesService = companiesService;
            _updateService = updateService;
            _banksProvidersProvider = banksProvidersProvider;
            _charactersProvider = charactersProvider;
            _companiesProvider = companiesProvider;
        }

        public async Task Init()
        {
            foreach (var characterData in _charactersProvider.GetAllCharacter())
            {
                _charactersIncomes.TryAdd(characterData.Key,CalculateCharacterIncomePerMonth(characterData));
            }

            _cleanBank = _banksProvidersProvider.GetFloatBankProvider().Get(BankId.CleanMoney);
            _dirtyBank = _banksProvidersProvider.GetFloatBankProvider().Get(BankId.DirtyMoney);
            _myCharacter = _charactersProvider.GetMyCharacter();
            
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
            var result = 0f;
            var company = _companiesProvider.GetCompanyByKey(companyKey);
            var ownerProducts = _charactersProvider.GetCharacterByKey(company.Owner).Products;

            foreach (var data in company.Products.Select(item => _productsProvider.GetProduct(item)))
                result += data.IncomePerMonth;
            
            foreach (var data in ownerProducts.Select(item => _productsProvider.GetProduct(item)))
                result += data.IncomePerMonth;
            
            foreach (var data in company.Employees.Select(item => _charactersProvider.GetCharacterByKey(item)))
                result += data.Salary;
            
            _companiesIncomes.TryAdd(companyKey, result);
        }

        public float GetProductIncomePerMonth(string productKey) 
            => _productsIncomes[productKey];

        private float CalculateCharacterIncomePerMonth(ICharacterData characterData)
        {
            float characterProductsIncome = 0f;
            if (characterData == null)
                return characterProductsIncome;
            foreach (string productKey in characterData.Products.Where(item =>
                         _productsProvider.GetProduct(item)?.ProductState == ProductState.Released))
            {
                var item = _productsProvider.GetProduct(productKey);
                characterProductsIncome += item.IncomePerMonth;
            }

            characterProductsIncome -= _companiesService.GetCompanyEmployeesSalary(characterData.CompanyKey);
            
            if(_charactersIncomes.ContainsKey(characterData.Key))
                _charactersIncomes[characterData.Key] = characterProductsIncome;
            
            if (characterData == _myCharacter)
                OnMyPlayerIncomeChanged?.Invoke();
            
            return characterProductsIncome;
        }

        private float GetIncomePerTick(float tick, ICharacterData characterData)
        {
            if (characterData == null)
                return 0f;
            _charactersIncomes.TryGetValue(characterData.Key, out var incomePerMonth);
            float result = ConvertMonthValueIntoTick(incomePerMonth) * tick;
            
            return result;
        }

        private static float ConvertMonthValueIntoTick(float incomePerMonth) 
            => incomePerMonth / 120;

        public void Tick(float tickTime)
        {
            var addValue = GetIncomePerTick(tickTime, _myCharacter);
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