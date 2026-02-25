using System;
using UI.Buttons;
using UnityEngine;

namespace Mechanics.Product
{
    public class ProductItem : MonoBehaviour
    {
        [field: SerializeField] public ProductViewsAdapter ProductViewsAdapter { get; private set; }

        [SerializeField] private GameObject _developingState;
        [SerializeField] private GameObject _releasedState;

        [SerializeField] private UIButton _button;
        private IContractData _contractData;

        public event Action<IContractData> OnProductItemClicked;

        private void Start()
        {
            _button.OnClicked += Button_OnClicked;
        }

        public void Init(IContractData contractData)
        {
            _contractData = contractData;

            if (_contractData.ContractState == ContractState.Executing)
            {
                _developingState.gameObject.SetActive(true);
                _releasedState.gameObject.SetActive(false);
                WaitUntilCompleted(contractData);
            }
            else
            {
                _developingState.gameObject.SetActive(false);
                _releasedState.gameObject.SetActive(true);
            }
        }

        private void WaitUntilCompleted(IContractData contractData)
        {
            contractData.OnCompleted += ContractData_OnCompleted;
        }

        private void ContractData_OnCompleted(IContractData contractData)
        {
            contractData.OnCompleted -= ContractData_OnCompleted;
            _developingState.gameObject.SetActive(false);
            _releasedState.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _button.OnClicked -= Button_OnClicked;
        }

        private void Button_OnClicked()
            => OnProductItemClicked?.Invoke(ProductViewsAdapter.ContractData);
    }
}