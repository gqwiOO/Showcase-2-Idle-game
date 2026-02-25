using TMPro;
using UnityEngine;

namespace Mechanics.Product
{
    public class ProductNameView : ProductView
    {
        [SerializeField] 
        private TMP_Text _text;
        
        public override void UpdateView() 
            => _text.text = contractData.Name;
    }
}