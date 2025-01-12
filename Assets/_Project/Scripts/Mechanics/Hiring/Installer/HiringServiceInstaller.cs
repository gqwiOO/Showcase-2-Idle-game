using Mechanics.Hiring.Service;
using Zenject;

namespace Mechanics.Hiring.Installer
{
    public class HiringServiceInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IHiringService>().To<HiringService>().AsSingle();
        }
    }
}