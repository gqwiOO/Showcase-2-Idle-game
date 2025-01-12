using TMPro;
using UnityEngine;

namespace Mechanics.Product
{
    public class ProductStateView : ProductView
    {
        [SerializeField] private TMP_Text _stateText;
        public override void UpdateView() 
            => _stateText.text = productData.ProductState.ToString();
    }
}