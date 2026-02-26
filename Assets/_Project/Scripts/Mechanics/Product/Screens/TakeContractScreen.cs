using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.Companies;
using Services.Screen;
using TMPro;
using UI.Buttons;
using UnityEngine;
using Zenject;

namespace Mechanics.Product
{
    public class TakeContractScreen : BaseScreen
    {
        [SerializeField] private TMP_Text _offerTypeText;
        [SerializeField] private TMP_Text _offerTierText;
        [SerializeField] private TMP_Text _rewardText;
        [SerializeField] private TMP_Text _reputationText;
        [SerializeField] private TMP_Text _heatText;
        [SerializeField] private TMP_Text _requiredRolesText;
        [SerializeField] private CreateProductDevelopersView _assigneesView;
        [SerializeField] private UIButton _addAssigneeButton;
        [SerializeField] private UIButton _takeButton;
        [SerializeField] private UIButton _closeButton;
        [SerializeField] private SelectEmployeesScreen selectEmployeesScreen;

        private IContractData _currentOffer;
        private readonly List<ICharacterData> _selectedAssignees = new();
        private ICharacterData _myCharacter;

        private IContractMarketService _marketService;
        private ICharactersProvider _charactersProvider;
        private ICompaniesService _companiesService;

        [Inject]
        private void Construct(IContractMarketService marketService, ICharactersProvider charactersProvider,
            ICompaniesService companiesService)
        {
            _marketService = marketService;
            _charactersProvider = charactersProvider;
            _companiesService = companiesService;
        }

        private void Start()
        {
            _addAssigneeButton.OnClicked += OnAddAssigneeClicked;
            _takeButton.OnClicked += OnTakeClicked;
            _closeButton.OnClicked += () => Hide().Forget();
            _assigneesView.OnDeveloperUnselected += OnAssigneeUnselected;
            selectEmployeesScreen.OnDeveloperSelected += OnAssigneeSelected;
        }

        private void OnDestroy()
        {
            _addAssigneeButton.OnClicked -= OnAddAssigneeClicked;
            _takeButton.OnClicked -= OnTakeClicked;
            _assigneesView.OnDeveloperUnselected -= OnAssigneeUnselected;
            selectEmployeesScreen.OnDeveloperSelected -= OnAssigneeSelected;
        }

        public void Init(IContractData offer)
        {
            _currentOffer = offer;
            _myCharacter = _charactersProvider.GetMyCharacter();
            _selectedAssignees.Clear();
            _assigneesView.ResetView();

            if (_offerTypeText != null) _offerTypeText.text = offer.Type.ToString();
            if (_offerTierText != null) _offerTierText.text = $"Tier {(int)offer.Tier}";
            if (_rewardText != null) _rewardText.text = $"{offer.RewardAmount:F0}$";
            if (_reputationText != null) _reputationText.text = $"+{offer.ReputationReward}";
            if (_heatText != null) _heatText.text = $"+{offer.HeatReward}";
            if (_requiredRolesText != null)
                _requiredRolesText.text = offer.RequiredRoles != null ? string.Join(", ", offer.RequiredRoles) : "";
            
            _takeButton.SetInteractableState(false);
        }

        private void OnAddAssigneeClicked()
        {
            var available = GetAvailableAssignees().Where(a => !_selectedAssignees.Contains(a)).ToList();
            var maxToAdd = 3 - _selectedAssignees.Count;
            selectEmployeesScreen.Open().Forget();
            selectEmployeesScreen.Init(available, maxToAdd);
        }

        private void OnAssigneeSelected(ICharacterData character)
        {
            _selectedAssignees.Add(character);
            _assigneesView.Add(character);
            _takeButton.SetInteractableState(true);
        }

        private void OnAssigneeUnselected(ICharacterData character)
        {
            _selectedAssignees.Remove(character);
            _assigneesView.Remove(character);
            
            if(_selectedAssignees.Count == 0)
                _takeButton.SetInteractableState(false);
        }

        private void OnTakeClicked()
        {
            if (_currentOffer == null || !_marketService.CanTakeOffer(_currentOffer)) return;

            _marketService.TakeOffer(_currentOffer, new List<ICharacterData>(_selectedAssignees), _myCharacter.Key);
            Hide().Forget();
        }

        private List<ICharacterData> GetAvailableAssignees()
        {
            var employees = new HashSet<ICharacterData>(_companiesService.GetCompanyEmployees(_myCharacter.CompanyKey));
            employees.Add(_myCharacter);
            return employees.Where(e => !e.IsInjured).ToList();
        }
    }
}
