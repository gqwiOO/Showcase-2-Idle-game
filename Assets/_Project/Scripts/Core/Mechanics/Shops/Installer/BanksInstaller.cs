using Zenject;

namespace Core.Mechanics.Shops
{
    public class BanksInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BanksFactory>().AsSingle();
            Container.BindInterfacesTo<BanksProvidersProvider>().AsSingle();
        }
    }
}