using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.Product
{
    public class ProductViewsAdapter : ProductView
    {
        [SerializeField] private List<ProductView> _views;

        public IContractData ContractData => base.contractData;

        public override void Init(IContractData data)
        {
            base.Init(data);
            _views.ForEach(view => view.Init(data));
        }

        public override void UpdateView()
            => _views.ForEach(view => view.UpdateView());
    }
}