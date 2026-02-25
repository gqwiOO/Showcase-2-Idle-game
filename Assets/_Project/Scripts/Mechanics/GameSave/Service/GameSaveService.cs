using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Mechanics.Shops;
using Core.Storage.Bank;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.Product;
using Mechanics.Product;
using Mechanics.Product.Provider;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;

namespace Mechanics.GameSave
{
    public class GameSaveService: IGameSaveService
    {
        private const string DATA_PATH = "/GameSave/";
        private const string DATA_NAME_PREFIX = "GameSaveData_";
        
        private readonly ICompaniesProvider _companiesProvider;
        private readonly ICharactersProvider _charactersProvider;
        private GameData _gameData;
        private IContractsProvider _contractsProvider;
        private IBanksProvidersProvider _banksProvidersProvider;
        
        private IDataBank<float> _cleanWallet;
        private IDataBank<float> _dirtyWallet;
        private IDataBank<float> _reputationWallet;
        private IDataBank<float> _heatWallet;

        public IEnumerable<ICompanyData> Companies => _gameData.Companies;
        public IEnumerable<ICharacterData> Characters  => _gameData.Characters;
        public ICharacterData MyCharacter  => _gameData.MyCharacter;
        public float MoneyAmount  => _gameData.CleanMoneyAmount;

        public GameSaveService(ICompaniesProvider companiesProvider, ICharactersProvider charactersProvider,
            IContractsProvider contractsProvider, IBanksProvidersProvider banksProvidersProvider)
        {
            _banksProvidersProvider = banksProvidersProvider;
            _contractsProvider = contractsProvider;
            _companiesProvider = companiesProvider;
            _charactersProvider = charactersProvider;
        }

        public async Task Init()
        {
            _cleanWallet = _banksProvidersProvider.GetFloatBankProvider().Get(BankId.CleanMoney);
            _dirtyWallet = _banksProvidersProvider.GetFloatBankProvider().Get(BankId.DirtyMoney);
            _reputationWallet = _banksProvidersProvider.GetFloatBankProvider().Get(BankId.Reputation);
            _heatWallet = _banksProvidersProvider.GetFloatBankProvider().Get(BankId.Heat);
            await LoadLastGame();
        }

        private string ConvertGameToJson()
        {
            var companies = _companiesProvider.GetAllCompanies();
            var characters = _charactersProvider.GetAllCharacter();
            var contracts = _contractsProvider.GetAllContracts();
            var myCharacter = (CharacterData)_charactersProvider.GetMyCharacter();

            var gameData = new GameData
            {
                Companies = new List<CompanyData>(companies.Select(item => (CompanyData)item)),
                Characters = new List<CharacterData>(characters.Select(item => (CharacterData)item)),
                Contracts = new List<ContractData>(contracts.Select(item => (ContractData)item)),
                MyCharacter = myCharacter,
                MoneyAmount = _cleanWallet.GetValue(),
                CleanMoneyAmount = _cleanWallet.GetValue(),
                DirtyMoneyAmount = _dirtyWallet.GetValue(),
                ReputationAmount = _banksProvidersProvider.GetFloatBankProvider().Get(BankId.Reputation).GetValue(),
                HeatAmount = _banksProvidersProvider.GetFloatBankProvider().Get(BankId.Heat).GetValue()
            };

            return  Newtonsoft.Json.JsonConvert.SerializeObject(gameData);
        }

        public async Task<GameData> LoadGame(int key)
        {
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
            {
                GameSaveDataJsonStorage storage = new GameSaveDataJsonStorage(DATA_PATH + DATA_NAME_PREFIX + key);
                storage.Load();
                var data = storage.Get();
                _gameData = data;
            });
            
            var cleanAmount = _gameData.CleanMoneyAmount > 0 ? _gameData.CleanMoneyAmount : _gameData.MoneyAmount;
            var dirtyAmount = _gameData.DirtyMoneyAmount;
            var reputationAmount = _gameData.ReputationAmount;
            var heatAmount = _gameData.HeatAmount;
            _cleanWallet.SetValue(cleanAmount);
            _dirtyWallet.SetValue(dirtyAmount);
            _reputationWallet.SetValue(reputationAmount);
            _heatWallet.SetValue(heatAmount);
            return _gameData;
        }

        private async Task<bool> HasAnySave()
        {
            string directoryPath = null;
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
            {
                directoryPath = Application.persistentDataPath + DATA_PATH;
            });
            
            if (!System.IO.Directory.Exists(directoryPath))
                return false;

            var files = System.IO.Directory.GetFiles(directoryPath, DATA_NAME_PREFIX + "*.json");
            return files.Length > 0;
        }

        public void SaveGame(int key)
        {
            var json = ConvertGameToJson();
            
            var directoryPath = Path.Combine(Application.persistentDataPath + DATA_PATH);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            
            var filePath = directoryPath + DATA_NAME_PREFIX + key + ".json";
            using (var writer = new System.IO.StreamWriter(filePath))
            {
                writer.Write(json);
            }
            
        }

        public void SaveGame()
        {
            SaveGame(0);
        }

        public GameData GetCurrentData()
        {
            return _gameData;
        }

        public async Task<GameData> LoadLastGame()
        {
            if (await HasAnySave())
            {
               var result =  await LoadGame(0);
               return result;
            }
            return null;
        } 

        public bool IsGameLoaded() => _gameData != null;
    }

    public interface IGameSaveService: IService
    {
        Task<GameData> LoadGame(int key);
        void SaveGame(int key);
        void SaveGame();
        GameData GetCurrentData();
        Task<GameData> LoadLastGame();
        
        IEnumerable<ICompanyData> Companies { get; }
        IEnumerable<ICharacterData> Characters { get; }
        ICharacterData MyCharacter { get; }
        float MoneyAmount { get; }

        bool IsGameLoaded();
    }
}