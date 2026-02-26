using Mechanics.Income.Service;
using Zenject;

namespace Mechanics.Income.Installer
{
    public class EconomyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameEconomyService>().AsSingle();
            Container.BindInterfacesTo<LaunderService>().AsSingle();
            Container.Bind<DayNightEconomyBridge>().AsSingle();
        }
    }
}