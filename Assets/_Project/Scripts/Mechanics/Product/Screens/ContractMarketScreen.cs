using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.Product.Provider;
using Services.Screen;
using Services.Screen.Interfaces;
using TMPro;
using UI.Buttons;
using UI.Toggles;
using UnityEngine;
using Zenject;

namespace Mechanics.Product
{
    public class ContractMarketScreen : BaseScreen
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private ContractMarketItemView _itemPrefab;
        [SerializeField] private TMP_Text _emptyText;
        [SerializeField] private UIButton _closeButton;
        [SerializeField] private BaseToggle _availableTab;
        [SerializeField] private BaseToggle _activeTab;

        private List<ContractMarketItemView> _activeItems = new();
        private bool _isAvailableTabSelected = true;

        private IContractMarketService _marketService;
        private IContractService _contractService;
        private ICharactersProvider _charactersProvider;
        private ICompaniesService _companiesService;
        private ICompaniesProvider _companiesProvider;
        private IContractsProvider _contractsProvider;
        private IMainScreenService _mainScreenService;

        [Inject]
        private void Construct(IContractMarketService marketService, IContractService contractService,
            ICharactersProvider charactersProvider, ICompaniesService companiesService,
            ICompaniesProvider companiesProvider, IContractsProvider contractsProvider,
            IMainScreenService mainScreenService)
        {
            _marketService = marketService;
            _contractService = contractService;
            _charactersProvider = charactersProvider;
            _companiesService = companiesService;
            _companiesProvider = companiesProvider;
            _contractsProvider = contractsProvider;
            _mainScreenService = mainScreenService;
        }

        private void Awake()
        {
            if (_closeButton != null)
                _closeButton.OnClicked += CloseButton_OnClicked;
            if (_availableTab != null)
                _availableTab.OnValidClick += AvailableTab_OnClick;
            if (_activeTab != null)
                _activeTab.OnValidClick += ActiveTab_OnClick;
        }

        private void OnDestroy()
        {
            if (_closeButton != null)
                _closeButton.OnClicked -= CloseButton_OnClicked;
            if (_availableTab != null)
                _availableTab.OnValidClick -= AvailableTab_OnClick;
            if (_activeTab != null)
                _activeTab.OnValidClick -= ActiveTab_OnClick;
        }

        private void CloseButton_OnClicked() => Hide().Forget();
        private void AvailableTab_OnClick() => SelectTab(available: true);
        private void ActiveTab_OnClick() => SelectTab(available: false);

        private void SelectTab(bool available)
        {
            _isAvailableTabSelected = available;
            RefreshView();
        }

        private void OnEnable()
        {
            _marketService.OnOffersRefreshed += RefreshView;
            _marketService.OnOfferTaken += RefreshView;
            _contractService.OnContractCompleted += OnContractCompleted;
            RefreshView();
        }

        private void OnDisable()
        {
            _marketService.OnOffersRefreshed -= RefreshView;
            _marketService.OnOfferTaken -= RefreshView;
            _contractService.OnContractCompleted -= OnContractCompleted;
        }

        private void OnContractCompleted(ContractCompletionResult _)
        {
            if (!_isAvailableTabSelected)
                RefreshView();
        }

        private void RefreshView()
        {
            if (_isAvailableTabSelected)
                RefreshAvailableTab();
            else
                RefreshActiveTab();
        }

        private void RefreshAvailableTab()
        {
            var offers = _marketService.GetAvailableOffers().ToList();

            if (_emptyText != null)
                _emptyText.gameObject.SetActive(offers.Count == 0);

            EnsureItemCount(offers.Count);

            for (int i = 0; i < offers.Count; i++)
            {
                var item = _activeItems[i];
                item.gameObject.SetActive(true);
                item.InitAsOffer(offers[i], _marketService, this);
            }

            HideExcessItems(offers.Count);
        }

        private void RefreshActiveTab()
        {
            var contractsInProgress = GetContractsInProgress().ToList();

            if (_emptyText != null)
                _emptyText.gameObject.SetActive(contractsInProgress.Count == 0);

            EnsureItemCount(contractsInProgress.Count);

            for (int i = 0; i < contractsInProgress.Count; i++)
            {
                var item = _activeItems[i];
                item.gameObject.SetActive(true);
                item.InitAsContract(contractsInProgress[i], this);
            }

            HideExcessItems(contractsInProgress.Count);
        }

        private IEnumerable<IContractData> GetContractsInProgress()
        {
            var myCharacter = _charactersProvider.GetMyCharacter();
            var contractKeys = new List<string>(myCharacter.Contracts);

            var company = _companiesProvider.GetCompanyByOwnerKey(myCharacter.Key);
            if (company != null)
                contractKeys.AddRange(company.Contracts);

            return contractKeys
                .Select(k => _contractsProvider.GetContract(k))
                .Where(c => c != null && c.ContractState == ContractState.Executing);
        }

        private void EnsureItemCount(int count)
        {
            while (_activeItems.Count < count)
            {
                var item = Instantiate(_itemPrefab, _container);
                _activeItems.Add(item);
            }
        }

        private void HideExcessItems(int usedCount)
        {
            for (int i = usedCount; i < _activeItems.Count; i++)
                _activeItems[i].gameObject.SetActive(false);
        }

        public void OnTakeOfferClicked(IContractData offer)
        {
            _mainScreenService.ShowTakeContractScreen(offer).Forget();
        }
    }
}
