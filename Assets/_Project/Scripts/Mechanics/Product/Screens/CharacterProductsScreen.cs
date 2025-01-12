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
        private IProductService _productService;
        private string _characterKey;
        private IProductsProvider _productsProvider;

        [Inject]
        private void Construct(ICompaniesProvider companiesProvider, ICharactersProvider charactersProvider, IMainScreenService mainScreenService,
            IProductService productService, IProductsProvider productsProvider)
        {
            _productsProvider = productsProvider;
            _productService = productService;
            _mainScreenService = mainScreenService;
            _charactersProvider = charactersProvider;
            _companiesProvider = companiesProvider;
        }

        private void Start()
        {
            _createGameButton.OnClicked += CreateButton_OnClicked;
            _productService.OnNewGameProductAdded += ProductService_OnNewGameProductCreated;
        }

        private void OnDestroy()
        {
            _createGameButton.OnClicked -= CreateButton_OnClicked;
        }

        private void ProductService_OnNewGameProductCreated(IGameProductData productData) 
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

            List<string> characterProducts = new(_characterData.Products);

            if (!_characterData.CompanyKey.IsEmpty())
            {
                var products = _companiesProvider.GetCompanyByOwnerKey(_characterData.Key).Products;
                characterProducts.AddRange(products);
            }

            await InitProducts(characterProducts);
        }

        private async UniTask InitProducts(List<string> characterProducts)
        {
            int productCount = characterProducts.Count;

            for (int i = 0; i < productCount; i++)
            {
                ProductViewsAdapter view;

                
                var productData = _productsProvider.GetProduct(characterProducts[i]);
                if (i < _activeViews.Count)
                {
                    view = _activeViews[i];
                }
                else
                {
                    var productItem = GetProductItem();
                    view = productItem.ProductViewsAdapter;
                    _activeViews.Add(view);
                    productItem.OnProductItemClicked += ProductionItem_OnClicked;
                    productItem.Init(productData);
                }

                view.gameObject.SetActive(true);
                view.Init(productData);
                view.UpdateView();
                
            }

            for (int i = productCount; i < _activeViews.Count; i++)
            {
                _activeViews[i].gameObject.SetActive(false);
            }
        }

        private void ProductionItem_OnClicked(IProductData data)
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
