using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.Developing.Data;
using Services.Screen;
using TMPro;
using UI.Buttons;
using UnityEngine;
using Zenject;

namespace Mechanics.Product
{
    public class CreateProductScreen: BaseScreen
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
        private SelectDevelopersScreen _selectDevelopersScreen;

        private ICharacterData _myCharacter;
        
        private GameGenre _currentGenre;
        private List<ICharacterData> _currentSelectedEmployees = new ();
        

        private IProductService _productService;
        private ICharactersProvider _charactersProvider;
        private ICompaniesService _companiesService;
        private ICompaniesProvider _companiesProvider;

        private const int MAX_DEVELOPERS_ON_PROJECT = 3;


        [Inject]
        private void Construct(IProductService productService, ICharactersProvider charactersProvider, ICompaniesService companiesService,
            ICompaniesProvider companiesProvider)
        {
            _companiesProvider = companiesProvider;
            _companiesService = companiesService;
            _charactersProvider = charactersProvider;
            _productService = productService;
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
            _selectDevelopersScreen.OnDeveloperSelected += SelectDeveloperScreen_OnDeveloperSelected;
            _createProductDevelopersView.OnDeveloperUnselected += CreateProductDevelopersView_OnDeveloperUnselected;
        }

        private void CreateProductDevelopersView_OnDeveloperUnselected(ICharacterData obj)
        {
            _currentSelectedEmployees.Remove(obj);
            _createProductDevelopersView.Remove(obj);
        }

        private void SelectDeveloperScreen_OnDeveloperSelected(ICharacterData data)
        {
            _currentSelectedEmployees.Add(data);
            _createProductDevelopersView.Add(data);
        }

        private void AddDeveloperButton_OnClicked()
        {
            _selectDevelopersScreen.Open().Forget();
            _selectDevelopersScreen.Init(GetNotSelectedEmployeesOnProduct(), MAX_DEVELOPERS_ON_PROJECT - _currentSelectedEmployees.Count);
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
            _nameText.text = _productService.GetRandomGameNameWithGenre(_currentGenre);
        }

        private void GameGenreDropDownView_OnGenreSelected(GameGenre obj) 
            => _currentGenre = obj;

        private void CreateButton_OnClicked()
        {
            CreateGameProductData createGameProductData = 
                new CreateGameProductData(_nameText.text,
                    _currentGenre,
                    new DevelopingData(_currentSelectedEmployees) ,
                    "TestProductKey_2");
            _productService.CreateGameProduct(createGameProductData,_myCharacter.Key);

            ResetView();
            
            Hide().Forget();
        }

        private void ResetView()
        {
            _createProductDevelopersView.ResetView();
        }
    }
}