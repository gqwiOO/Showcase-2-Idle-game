using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Mechanics.Shops;
using Core.Storage.Bank;
using Mechanics.Characters;
using Mechanics.Companies;
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
        private IProductsProvider _productsProvider;
        private IBanksProvidersProvider _banksProvidersProvider;
        
        private IDataBank<float> _wallet;

        public IEnumerable<ICompanyData> Companies => _gameData.Companies;
        public IEnumerable<ICharacterData> Characters  => _gameData.Characters;
        public ICharacterData MyCharacter  => _gameData.MyCharacter;
        public float MoneyAmount  => _gameData.MoneyAmount;

        public GameSaveService(ICompaniesProvider companiesProvider, ICharactersProvider charactersProvider,
            IProductsProvider productsProvider, IBanksProvidersProvider banksProvidersProvider)
        {
            _banksProvidersProvider = banksProvidersProvider;
            _productsProvider = productsProvider;
            _companiesProvider = companiesProvider;
            _charactersProvider = charactersProvider;
        }

        public async Task Init()
        {
            _wallet = _banksProvidersProvider.GetFloatBankProvider().Get(BankId.FruitsBank);
            await LoadLastGame();

        }

        private string ConvertGameToJson()
        {
            var companies = _companiesProvider.GetAllCompanies();
            var characters = _charactersProvider.GetAllCharacter();
            var products = _productsProvider.GetAllProduct();
            var myCharacter = (CharacterData)_charactersProvider.GetMyCharacter();

            var gameData = new GameData
            {
                Companies = new List<CompanyData>(companies.Select(item => (CompanyData)item)),
                Characters = new List<CharacterData>(characters.Select(item => (CharacterData)item)),
                Products = new List<GameProductData>(products.Select(item => (GameProductData)item)),
                MyCharacter = myCharacter,
                MoneyAmount = _wallet.GetValue()
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
            
            _wallet.SetValue(_gameData.MoneyAmount);
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