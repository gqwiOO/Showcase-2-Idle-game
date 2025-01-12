using Mechanics.Product.Provider;
using Zenject;

namespace Mechanics.Product
{
    public class ProductsInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ProductService>().AsSingle();
            Container.BindInterfacesTo<ProductsProvider>().AsSingle();
        }
    }
}