using Zenject;

namespace Mechanics.Events.Installer
{
    public class EventsInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EventsService>().AsSingle();
            
            InstallCompanyEvents();
        }

        private void InstallCompanyEvents()
        {
            Container.Bind<ICompanyEventInvoker>().To<CompanyEventInvoker>().AsSingle();
            Container.Bind<ICompanyEventHandler>().To<CompanyEventHandler>().AsSingle();
            Container.Bind<IEventsGenerator>().To<EventsGenerator>().AsSingle();
        }
    }
}