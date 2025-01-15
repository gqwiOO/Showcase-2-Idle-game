using UnityEngine;
using Zenject;

namespace Mechanics.Events.Installer
{
    public class EventHandlersInstaller: MonoInstaller
    {
        [SerializeField] private CompanyEventHandler _companyEventHandler;
        
        public override void InstallBindings()
        {
            Container.Bind<ICompanyEventHandler>().To<CompanyEventHandler>().FromInstance(_companyEventHandler).AsSingle();
        }
    }
}