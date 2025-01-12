using Core.Scripts.Services.UpdateService;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Installers.Core
{
    public class UpdateServiceInstaller: MonoInstaller
    {
        [SerializeField] private UpdateService updateService;
        
        public override void InstallBindings() 
            => Container.Bind<IUpdateService>().To<UpdateService>().FromInstance(updateService);
    }
}