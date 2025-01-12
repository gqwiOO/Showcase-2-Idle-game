using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.Income.View;
using Services.Screen;
using Services.Screen.Interfaces;
using UI.Toggles;
using UnityEngine;
using Zenject;

namespace Mechanics.MainMenu.Screens
{
    public class MainMenuScreen: BaseScreen
    {
        [SerializeField] private BaseToggle _productsToggle;
        [SerializeField] private BaseToggle _roomToggle;
        [SerializeField] private BaseToggle _hireToggle;
        [SerializeField] private BaseToggle _companyToggle;
        [SerializeField] private CharacterTotalIncomePerMonthView _incomeView;
        private IMainScreenService _mainScreenService;
        private ICharactersProvider _charactersProvider;
        private ICharacterData _myCharacterData;
        private ICompaniesProvider _companiesProvider;

        [Inject]
        private void Construct(IMainScreenService mainScreenService, ICharactersProvider charactersProvider, ICompaniesProvider companiesProvider)
        {
            _companiesProvider = companiesProvider;
            _charactersProvider = charactersProvider;
            _mainScreenService = mainScreenService;
        }

        private void Start()
        {
            _productsToggle.OnValidClick += ProductsToggle_OnClick;
            _roomToggle.OnValidClick += RoomToggle_OnClick;
            _hireToggle.OnValidClick += HireToggle_OnClick;
            _companyToggle.OnValidClick += CompanyToggle_OnClick;
                
            _myCharacterData = _charactersProvider.GetMyCharacter();
            _incomeView.Init(_myCharacterData);
        }

        private void CompanyToggle_OnClick()
        {
            _mainScreenService.HideAllScreens();
            _mainScreenService.ShowCompanyScreen(_companiesProvider.GetCompanyByOwnerKey(_myCharacterData.Key));
        }

        private void HireToggle_OnClick()
        {
            _mainScreenService.HideAllScreens();
            _mainScreenService.ShowHiringScreen();
        }

        private void RoomToggle_OnClick()
        {
            _mainScreenService.HideAllScreens();
        }

        private void OnDestroy()
        {
            _productsToggle.OnValidClick -= ProductsToggle_OnClick;
            _roomToggle.OnValidClick -= RoomToggle_OnClick;
            _hireToggle.OnValidClick -= HireToggle_OnClick;
            _companyToggle.OnValidClick -= CompanyToggle_OnClick;
        }
        
        private void ProductsToggle_OnClick()
        {
            _mainScreenService.HideAllScreens();
            _mainScreenService.ShowCharacterProductsScreen(_myCharacterData.Key);
        }
    }
}