using UnityEngine;
using Zenject;

namespace Core.Scripts.Services.Tutorial.Installer
{
    public class TutorialHandlerInstaller : MonoInstaller
    {
        [SerializeField] private TutorialHandler _tutorialHandler;
        
        
        public override void InstallBindings()
        {
            Container.Bind<ITutorialHandler>().To<TutorialHandler>().FromInstance(_tutorialHandler).AsSingle();
        }
    }
}