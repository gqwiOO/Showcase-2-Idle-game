using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.Companies
{
    public class CompanyViewsAdapter: BaseCompanyView
    {
        [SerializeField] private List<BaseCompanyView> _companyViews;
        
        public override void Init(ICompanyData companyData)
        {
            base.Init(companyData);
            _companyViews.ForEach(view => view.Init(companyData));
        }

        public override void UpdateView() 
            => _companyViews.ForEach(view => view.UpdateView());
    }
}