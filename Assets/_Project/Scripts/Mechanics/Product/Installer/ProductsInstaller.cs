using Mechanics.Product.Provider;
using Zenject;

namespace Mechanics.Product
{
    public class ProductsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ContractService>().AsSingle();
            Container.BindInterfacesTo<ContractsProvider>().AsSingle();
            Container.BindInterfacesTo<ContractMarketService>().AsSingle();
        }
    }
}