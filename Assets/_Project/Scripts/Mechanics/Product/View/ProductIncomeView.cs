using TMPro;
using UnityEngine;

namespace Mechanics.Product
{
    public class ProductIncomeView : ProductView
    {
        [SerializeField] 
        private TMP_Text _text;

        [SerializeField]
        private string suffix;

        public override void UpdateView()
            => _text.text = contractData.RewardAmount.ToString("0.00") + suffix;
    }
}