using System.Threading.Tasks;
using Core.Mechanics.Shops;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.CompaniesRating.Service;
using Mechanics.DataSettings.Provider;
using Mechanics.DayNight;
using Mechanics.GameSave;
using Mechanics.Hiring.Service;
using Mechanics.Income;
using Mechanics.Product;
using PimDeWitte.UnityMainThreadDispatcher;
using Zenject;

namespace Services.ServiceInitializer
{
    public class ServiceInitializer : IServiceInitializer
    {
        private IGameEconomyService _gameEconomyService;
        private IGameTimeService _gameTimeService;
        private ICharactersService _charactersService;
        private IHiringService _hiringService;
        private ISettingsInitializer _settingsInitializer;
        private ICompaniesRatingService _companiesRatingService;
        private IAutoSaveService _autoSaveService;
        private IGameSaveService _gameSaveService;
        private ICompaniesService _companiesService;
        private IContractService _contractService;
        private IBanksProvidersProvider _banksProvidersProvider;
        private IBanksFactory _banksFactory;
        private IContractMarketService _contractMarketService;

        [Inject]
        private void Construct(IGameEconomyService gameEconomyService, IGameTimeService gameTimeService, ICharactersService charactersService, IHiringService hiringService,
            ISettingsInitializer settingsInitializer, ICompaniesRatingService companiesRatingService, IAutoSaveService autoSaveService,
            IGameSaveService gameSaveService, ICompaniesService companiesService, IContractService contractService,
            IBanksProvidersProvider banksProvidersProvider, IBanksFactory banksFactory, IContractMarketService contractMarketService
            )
        {
            _contractMarketService = contractMarketService;
            _banksFactory = banksFactory;
            _banksProvidersProvider = banksProvidersProvider;
            _contractService = contractService;
            _companiesService = companiesService;
            _gameSaveService = gameSaveService;
            _autoSaveService = autoSaveService;
            _companiesRatingService = companiesRatingService;
            _settingsInitializer = settingsInitializer;
            _hiringService = hiringService;
            _charactersService = charactersService;
            _gameTimeService = gameTimeService;
            _gameEconomyService = gameEconomyService;
        }
        public async Task Init()
        {
            InitBanks();
            
            await _gameSaveService.Init();
            await _settingsInitializer.Init();
            await InitCharacters();
            await _autoSaveService.Init();
            await _hiringService.Init();
            await InitCompanies();
            await InitContracts();
            await _companiesRatingService.Init();
            await _gameEconomyService.Init();
            await _contractMarketService.Init();
            _gameTimeService.Init();
        }

        private void InitBanks()
        {
            _banksFactory.CreateFloatBankWithId(BankId.CleanMoney, 0);
            _banksFactory.CreateFloatBankWithId(BankId.DirtyMoney, 0);
            _banksFactory.CreateFloatBankWithId(BankId.Reputation, 0);
            _banksFactory.CreateFloatBankWithId(BankId.Heat, 0);
        }

        private async Task InitCompanies()
        {
            if(_gameSaveService.IsGameLoaded())
                _companiesService.InjectGameSave(_gameSaveService.GetCurrentData());
            await _companiesService.Init();
        }
        private async Task InitContracts()
        {
            if (_gameSaveService.IsGameLoaded())
                await UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
                    _contractService.InjectGameSave(_gameSaveService.GetCurrentData()));
            // await _productService.Init();
        }
        
        private async Task InitCharacters()
        {
            if (_gameSaveService.IsGameLoaded())
                _charactersService.InjectGameSave(_gameSaveService.GetCurrentData());
            await _charactersService.Init();
        } 
    }
}