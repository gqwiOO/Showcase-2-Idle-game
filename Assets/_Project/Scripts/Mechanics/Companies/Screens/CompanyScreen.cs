using Core.Scripts.Debugging;
using Mechanics.Characters;
using Mechanics.Income;
using ModestTree;
using Services.Screen;
using TMPro;
using UI.Buttons;
using UI.Toggles;
using UnityEngine;
using Zenject;

namespace Mechanics.Companies
{
    public class CompanyScreen: BaseScreen
    {
        [Header("Containers")]
        [SerializeField] 
        private RectTransform _noCompanyContainer;
        [SerializeField] 
        private RectTransform _hasCompanyContainer;

        [Header("Tabs")] 
        [SerializeField] private BaseToggle _employeeTab;
        [SerializeField] private BaseToggle _productsTab;
        
        [Header("Buttons")]
        [SerializeField]
        private UIButton _createCompanyButton;

        [Header("Views")]
        [SerializeField] 
        private CompanyViewsAdapter _companyViewsAdapter;
        
        [SerializeField] 
        private TMP_Text _priceText;
        
        [Header("Data")]
        [SerializeField] 
        private float price;

        [SerializeField]
        private CompanyEmployeesView _companyEmployeesView;

        [Space]
        [SerializeField] private TMP_InputField _companyInputFieldName;
        
        private ICharacterData _myCharacter;
        private ICompanyData _companyData;
        
        private ICharactersProvider _charactersProvider;
        private ICompaniesService _companiesService;
        private IGameEconomyService _gameEconomyService;

        [Inject]
        private void Construct(ICharactersProvider charactersProvider, ICompaniesService companiesService, IGameEconomyService gameEconomyService)
        {
            _gameEconomyService = gameEconomyService;
            _companiesService = companiesService;
            _charactersProvider = charactersProvider;
        }

        public void Init(ICompanyData companyData)
        {
            _companyData = companyData;
            _myCharacter = _charactersProvider.GetMyCharacter();

            SetContainersState();

            _priceText.text = price + "$";
        }
        
        private void Start()
        {
            _createCompanyButton.OnClicked += CreateCompanyButton_OnClicked;
        }

        private void OnDestroy()
        {
            _createCompanyButton.OnClicked -= CreateCompanyButton_OnClicked;
        }

        private void CreateCompanyButton_OnClicked()
        {
            if (_companyInputFieldName.text.IsEmpty())
                Debugging.Log(this, "Company name is empty!");
            else
            {
                var company = _companiesService.CreateCompany(new CompanyCreateData(_companyInputFieldName.text, _myCharacter.Key));
                UpdateData(company);
                SetContainersState();
            }
        }

        private void UpdateData(ICompanyData company)
        {
            _companyData = company;
            
            _companyViewsAdapter.Init(_companyData);
            _companyViewsAdapter.UpdateView();
        }

        private void SetContainersState()
        {
            if (_companyData == null)
            {
                _noCompanyContainer.gameObject.SetActive(true);
                _hasCompanyContainer.gameObject.SetActive(false);

                _gameEconomyService.OnPlayerBalanceChanged += GameEconomyService_OnPlayerBalanceChanged;
            }
            else
            {
                _noCompanyContainer.gameObject.SetActive(false);
                _hasCompanyContainer.gameObject.SetActive(true);
                
                _companyEmployeesView.Init(_companyData);
                _gameEconomyService.OnPlayerBalanceChanged -= GameEconomyService_OnPlayerBalanceChanged;
            }
        }

        private void GameEconomyService_OnPlayerBalanceChanged(float value) 
            => _createCompanyButton.SetInteractableState(value >= price);
    }
}