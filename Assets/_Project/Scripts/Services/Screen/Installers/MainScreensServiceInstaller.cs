using UnityEngine;
using Zenject;

namespace Services.Screen.Installers
{
    public class MainScreensServiceInstaller: MonoInstaller
    {
        [SerializeField] private MainScreensService _mainScreensService;
        
        public override void InstallBindings() 
            => Container.BindInterfacesTo<MainScreensService>().FromInstance(_mainScreensService).AsSingle();
    }
}