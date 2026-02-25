using UnityEngine;
using UnityEngine.UI;

namespace Mechanics.Product
{
    public class ProductDevelopingBarView: ProductView
    {
        [SerializeField] private Image _barImage;

        public override void Init(IContractData data)
        {
            if (contractData is { ExecutionData: not null })
                contractData.ExecutionData.OnProgressUpdated -= ExecutionData_OnProgressUpdated;

            base.Init(data);

            if (contractData.ExecutionData != null)
            {
                contractData.ExecutionData.OnProgressUpdated += ExecutionData_OnProgressUpdated;
                ExecutionData_OnProgressUpdated(contractData.ExecutionData.CurrentProgress);
            }
        }

        private void ExecutionData_OnProgressUpdated(float newValue)
            => _barImage.fillAmount = newValue;

        public override void UpdateView()
        {
            
        }
    }
}