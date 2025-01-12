using Cysharp.Threading.Tasks;
using Services.Screen;
using UnityEngine;

namespace Mechanics.Product
{
    public class ProductManageScreen: BaseScreen
    {
        [SerializeField] private ProductViewsAdapter _productViewsAdapter;
        
        public async UniTask Init(IProductData productData)
        {
            _productViewsAdapter.Init(productData);
            _productViewsAdapter.UpdateView();
        }
    }
}