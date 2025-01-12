using Core.Mechanics.Shops.Provider;
using Zenject;

namespace Core.Mechanics.Shops.Installer
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