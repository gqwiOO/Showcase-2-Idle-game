using System;
using UI.Buttons;
using UnityEngine;

namespace Mechanics.Product
{
    public class ProductItem: MonoBehaviour
    {
        [field: SerializeField] public ProductViewsAdapter ProductViewsAdapter { get; private set; }
        
        [SerializeField]
        private GameObject _developingState;
        [SerializeField]
        private GameObject _releasedState;
        
        [SerializeField] private UIButton _button;
        private IProductData _productData;

        public event Action<IProductData> OnProductItemClicked;

        private void Start()
        {
            _button.OnClicked += Button_OnClicked;
        }

        public void Init(IProductData productData)
        {
            _productData = productData;

            if (_productData.ProductState == ProductState.Developing)
            {
                _developingState.gameObject.SetActive(true);
                _releasedState.gameObject.SetActive(false);

                WaitUntilReleased(productData);
            }
            else
            {
                _developingState.gameObject.SetActive(false);
                _releasedState.gameObject.SetActive(true);
            }
        }

        private void WaitUntilReleased(IProductData productData)
        {
            productData.OnReleased += ProductData_OnReleased;
        }

        private void ProductData_OnReleased(IProductData productData)
        {
            productData.OnReleased -= ProductData_OnReleased;
            
            _developingState.gameObject.SetActive(false);
            _releasedState.gameObject.SetActive(true);
        }
        

        private void OnDestroy()
        {
            _button.OnClicked -= Button_OnClicked;
        }

        private void Button_OnClicked() 
            => OnProductItemClicked?.Invoke(ProductViewsAdapter.ProductData);
    }
}