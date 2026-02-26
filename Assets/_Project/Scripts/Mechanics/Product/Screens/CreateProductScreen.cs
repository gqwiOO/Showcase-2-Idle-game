using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.Config;
using Mechanics.Developing.Data;
using Services.Screen;
using TMPro;
using UI.Buttons;
using UnityEngine;
using Zenject;

namespace Mechanics.Product
{
    public class CreateProductScreen : BaseScreen
    {
        [Header("Buttons")]
        [SerializeField] 
        private UIButton _createButton;
        
        [SerializeField] 
        private UIButton _addDeveloperButton;
        
        [SerializeField] 
        private UIButton _changeNameButton;

        [Header("Views")]
        [SerializeField] 
        private GameGenreDropDownView _gameGenreDropDownView;
        
        [SerializeField] 
        private CreateProductDevelopersView _createProductDevelopersView;
        
        [SerializeField] 
        private TMP_Text _nameText;

        [Space]
        [SerializeField] 
        private SelectEmployeesScreen selectEmployeesScreen;

        private ICharacterData _myCharacter;
        
        private GameGenre _currentGenre;
        private readonly List<ICharacterData> _currentSelectedEmployees = new ();
        

        private IContractService _contractService;
        private ICharactersProvider _charactersProvider;
        private ICompaniesService _companiesService;
        private ICompaniesProvider _companiesProvider;
        private ContractProductLimitsConfig _limitsConfig;

        [Inject]
        private void Construct(IContractService contractService, ICharactersProvider charactersProvider,
            ICompaniesService companiesService, ICompaniesProvider companiesProvider,
            ContractProductLimitsConfig limitsConfig)
        {
            _companiesProvider = companiesProvider;
            _companiesService = companiesService;
            _charactersProvider = charactersProvider;
            _contractService = contractService;
            _limitsConfig = limitsConfig;
        }

        public async UniTask Init()
        {
            _gameGenreDropDownView.Init();
            _myCharacter = _charactersProvider.GetMyCharacter();
            _gameGenreDropDownView.ManualSet(GameGenre.HyperCasual);
            GenerateGameName();
        }

        private void Start()
        {
            selectEmployeesScreen.OnDeveloperSelected += SelectEmployeeScreenOnEmployeeSelected;
            _createProductDevelopersView.OnDeveloperUnselected += CreateProductDevelopersView_OnDeveloperUnselected;
        }

        private void CreateProductDevelopersView_OnDeveloperUnselected(ICharacterData obj)
        {
            _currentSelectedEmployees.Remove(obj);
            _createProductDevelopersView.Remove(obj);
        }

        private void SelectEmployeeScreenOnEmployeeSelected(ICharacterData data)
        {
            _currentSelectedEmployees.Add(data);
            _createProductDevelopersView.Add(data);
        }

        private void AddDeveloperButton_OnClicked()
        {
            selectEmployeesScreen.Open().Forget();
            selectEmployeesScreen.Init(GetNotSelectedEmployeesOnProduct(), _limitsConfig.MaxDevelopersOnProject - _currentSelectedEmployees.Count);
        }

        private List<ICharacterData> GetNotSelectedEmployeesOnProduct()
        {
            HashSet<ICharacterData> companyEmployees = new HashSet<ICharacterData>(_companiesService.GetCompanyEmployees(_myCharacter.CompanyKey));
            companyEmployees.Add(_myCharacter);
            var result = companyEmployees
                .Where(item => !_currentSelectedEmployees.Contains(item)).ToList();
            return result;
        }

        private void ChangeNameButton_OnClicked() 
            => GenerateGameName();

        private void Awake()
        {
            _gameGenreDropDownView.OnGenreSelected += GameGenreDropDownView_OnGenreSelected;
            _createButton.OnClicked += CreateButton_OnClicked;
            _changeNameButton.OnClicked += ChangeNameButton_OnClicked;
            _addDeveloperButton.OnClicked += AddDeveloperButton_OnClicked;
        }

        private void OnDestroy()
        {
            _gameGenreDropDownView.OnGenreSelected -= GameGenreDropDownView_OnGenreSelected;
            _createButton.OnClicked -= CreateButton_OnClicked;
            _changeNameButton.OnClicked -= ChangeNameButton_OnClicked;
            _addDeveloperButton.OnClicked -= AddDeveloperButton_OnClicked;
        }

        private void GenerateGameName()
        {
            _nameText.text = _contractService.GetRandomContractNameWithGenre(_currentGenre);
        }

        private void GameGenreDropDownView_OnGenreSelected(GameGenre obj) 
            => _currentGenre = obj;

        private void CreateButton_OnClicked()
        {
            var contract = ContractData.Create(
                Guid.NewGuid().ToString(),
                _nameText.text,
                new DevelopingData(_currentSelectedEmployees.Select(item => (CharacterData)item).ToList()),
                _limitsConfig.DefaultRewardAmount,
                _limitsConfig.DefaultReputationReward,
                _limitsConfig.DefaultHeatReward);

            _contractService.TakeContract(contract, _myCharacter.Key);

            ResetView();
            Hide().Forget();
        }

        private void ResetView()
        {
            _createProductDevelopersView.ResetView();
        }
    }
}