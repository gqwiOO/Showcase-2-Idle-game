using Zenject;

namespace Core.Scripts.Services.Tutorial.Installer
{
    public class TutorialInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ITutorialService>().To<TutorialService>().AsSingle();
        }
    }
}