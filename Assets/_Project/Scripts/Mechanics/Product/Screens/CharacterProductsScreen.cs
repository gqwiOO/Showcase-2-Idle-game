using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Pools;
using Cysharp.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.Product.Provider;
using ModestTree;
using Services.Screen;
using Services.Screen.Interfaces;
using UI.Buttons;
using UnityEngine;
using Zenject;

namespace Mechanics.Product
{
    public class CharacterProductsScreen : BaseScreen
    {
        [Header("Buttons")] 
        [SerializeField] private UIButton _createGameButton;
        
        [Space]
        
        [SerializeField] private PoolGameObjects _productsPool;
        [SerializeField] private RectTransform _container;
        [SerializeField] private List<ProductViewsAdapter> _activeViews = new();

        private ICharacterData _characterData;
        private ICompaniesProvider _companiesProvider;
        private ICharactersProvider _charactersProvider;
        private IMainScreenService _mainScreenService;
        private IContractService _contractService;
        private string _characterKey;
        private IContractsProvider _contractsProvider;

        [Inject]
        private void Construct(ICompaniesProvider companiesProvider, ICharactersProvider charactersProvider, IMainScreenService mainScreenService,
            IContractService contractService, IContractsProvider contractsProvider)
        {
            _contractsProvider = contractsProvider;
            _contractService = contractService;
            _mainScreenService = mainScreenService;
            _charactersProvider = charactersProvider;
            _companiesProvider = companiesProvider;
        }

        private void Start()
        {
            _createGameButton.OnClicked += CreateButton_OnClicked;
            _contractService.OnContractTaken += ContractService_OnContractTaken;
        }

        private void OnDestroy()
        {
            _createGameButton.OnClicked -= CreateButton_OnClicked;
        }

        private void ContractService_OnContractTaken(IContractData contractData)
            => Init(_characterKey).Forget();

        private void CreateButton_OnClicked()
        {
            _mainScreenService.ShowCreateProductScreen();
        }

        public async UniTask Init(string characterKey)
        {
            _characterKey = characterKey;
            await Init(_charactersProvider.GetCharacterByKey(characterKey));
        }

        public async UniTask Init(ICharacterData characterData)
        {
            _productsPool.Init();
            _characterData = characterData;

            List<string> characterContracts = new(_characterData.Contracts);

            if (!_characterData.CompanyKey.IsEmpty())
            {
                var contracts = _companiesProvider.GetCompanyByOwnerKey(_characterData.Key).Contracts;
                characterContracts.AddRange(contracts);
            }

            await InitContracts(characterContracts);
        }

        private async UniTask InitContracts(List<string> characterContracts)
        {
            int contractCount = characterContracts.Count;

            for (int i = 0; i < contractCount; i++)
            {
                ProductViewsAdapter view;
                var contractData = _contractsProvider.GetContract(characterContracts[i]);
                if (contractData == null) continue;

                if (i < _activeViews.Count)
                {
                    view = _activeViews[i];
                }
                else
                {
                    var productItem = GetProductItem();
                    view = productItem.ProductViewsAdapter;
                    _activeViews.Add(view);
                    productItem.OnProductItemClicked += ContractItem_OnClicked;
                    productItem.Init(contractData);
                }

                view.gameObject.SetActive(true);
                view.Init(contractData);
                view.UpdateView();
            }

            for (int i = contractCount; i < _activeViews.Count; i++)
            {
                _activeViews[i].gameObject.SetActive(false);
            }
        }

        private void ContractItem_OnClicked(IContractData data)
        {
        }

        private ProductItem GetProductItem()
        {
            var result = _productsPool.Pull().GetOwner<ProductItem>();
            result.transform.SetParent(_container, false);
            return result;
        }
    }
}
