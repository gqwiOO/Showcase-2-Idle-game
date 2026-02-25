using UI.View;

namespace Mechanics.Product
{
    public abstract class ProductView: BaseView
    {
        protected IContractData contractData;

        public virtual void Init(IContractData data)
        {
            contractData = data;
        }

    }
}