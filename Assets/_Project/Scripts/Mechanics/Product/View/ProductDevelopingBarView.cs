using UnityEngine;
using UnityEngine.UI;

namespace Mechanics.Product
{
    public class ProductDevelopingBarView: ProductView
    {
        [SerializeField] private Image _barImage;

        public override void Init(IProductData data)
        {
            if(productData is { DevelopingData: not null }) 
                productData.DevelopingData.OnProgressUpdated -= DevelopingData_OnProgressUpdated;

            base.Init(data);

            if (productData.DevelopingData != null)
            {
                productData.DevelopingData.OnProgressUpdated += DevelopingData_OnProgressUpdated;
                DevelopingData_OnProgressUpdated(productData.DevelopingData.CurrentProgress);
            }
        }

        private void DevelopingData_OnProgressUpdated(float newValue) 
            => _barImage.fillAmount = newValue;

        public override void UpdateView()
        {
            
        }
    }
}