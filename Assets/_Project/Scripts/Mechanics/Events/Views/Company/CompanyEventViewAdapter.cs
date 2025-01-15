using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.Events.Views.Company
{
    public class CompanyEventViewAdapter : BaseCompanyEventView
    {
        [SerializeField] private List<BaseCompanyEventView> _views;

        public override void Init(CompanyEventData companyEventData)
        {
            base.Init(companyEventData);
            
            foreach (var view in _views)
                view.Init(companyEventData);
        }

        public override void UpdateView()
        {
            foreach (var view in _views)
            {
                view.UpdateView();
            }
        }
    }
}