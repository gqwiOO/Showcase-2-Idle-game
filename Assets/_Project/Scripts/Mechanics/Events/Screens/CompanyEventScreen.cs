using Mechanics.Events.Views.Company;
using UnityEngine;

namespace Mechanics.Events.Screens
{
    public class CompanyEventScreen : BaseEventScreen<CompanyEventData>
    {
        [SerializeField] private CompanyEventViewAdapter _eventView;
        
        protected override void InitView()
        {
            _eventView.Init(_data);
            _eventView.UpdateView();
        }
    }
}