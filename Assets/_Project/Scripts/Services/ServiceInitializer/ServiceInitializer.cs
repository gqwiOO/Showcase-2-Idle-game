using System.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.CompaniesRating.Service;
using Mechanics.DataSettings.Provider;
using Mechanics.GameSave;
using Mechanics.Hiring.Service;
using Mechanics.Income;
using Zenject;

namespace Services.ServiceInitializer
{
    public class ServiceInitializer : IServiceInitializer
    {
        private IGameEconomyService _gameEconomyService;
        private ICharactersService _charactersService;
        private IHiringService _hiringService;
        private ISettingsInitializer _settingsInitializer;
        private ICompaniesRatingService _companiesRatingService;
        private IAutoSaveService _autoSaveService;
        private IGameSaveService _gameSaveService;

        [Inject]
        private void Construct(IGameEconomyService gameEconomyService, ICharactersService charactersService, IHiringService hiringService,
            ISettingsInitializer settingsInitializer, ICompaniesRatingService companiesRatingService, IAutoSaveService autoSaveService,
            IGameSaveService gameSaveService
            )
        {
            _gameSaveService = gameSaveService;
            _autoSaveService = autoSaveService;
            _companiesRatingService = companiesRatingService;
            _settingsInitializer = settingsInitializer;
            _hiringService = hiringService;
            _charactersService = charactersService;
            _gameEconomyService = gameEconomyService;
        }
        public async Task Init()
        {
            await _gameSaveService.Init();
            await _settingsInitializer.Init();
            await _charactersService.Init();
            await _gameEconomyService.Init();
            await _autoSaveService.Init();
            await _hiringService.Init();
            await _companiesRatingService.Init();
        }
    }
}