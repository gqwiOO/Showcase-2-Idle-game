using UI.View;

namespace Mechanics.Companies
{
    public abstract class BaseCompanyView: BaseView
    {
        protected ICompanyData _companyData;

        public virtual void Init(ICompanyData companyData)
        {
            _companyData = companyData;
        }
    }
}