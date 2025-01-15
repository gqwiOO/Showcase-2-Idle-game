using UnityEngine;

namespace Mechanics.Events.Views.Company
{
    public abstract class BaseCompanyEventView: MonoBehaviour
    {
        protected CompanyEventData _companyEventData;

        public virtual void Init(CompanyEventData companyEventData)
        {
            _companyEventData = companyEventData;
        }

        public abstract void UpdateView();
    }
}