using Zenject;

namespace Mechanics.GameSave
{
    public class GameSaveServiceInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameSaveService>().To<GameSaveService>().AsSingle();
            Container.Bind<IAutoSaveService>().To<AutoSaveService>().AsSingle();
        }
    }
}