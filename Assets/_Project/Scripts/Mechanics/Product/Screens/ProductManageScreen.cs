using Cysharp.Threading.Tasks;
using Services.Screen;
using UnityEngine;

namespace Mechanics.Product
{
    public class ProductManageScreen: BaseScreen
    {
        [SerializeField] private ProductViewsAdapter _productViewsAdapter;
        
        public async UniTask Init(IContractData contractData)
        {
            _productViewsAdapter.Init(contractData);
            _productViewsAdapter.UpdateView();
        }
    }
}