using UI.View;

namespace Mechanics.Product
{
    public abstract class ProductView: BaseView
    {
        protected IProductData productData;

        public virtual void Init(IProductData data)
        {
            productData = data;
        }

    }
}