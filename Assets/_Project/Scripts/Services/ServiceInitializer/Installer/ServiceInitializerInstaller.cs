using Zenject;

namespace Services.ServiceInitializer
{
    public class ServiceInitializerInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IServiceInitializer>().To<ServiceInitializer>().AsSingle();
        }
    }
}