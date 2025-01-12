using Mechanics.DataSettings.Provider;
using Zenject;

namespace Mechanics.DataSettings.Installer
{
    public class GameSettingsInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISettingsProvider>().To<SettingsProvider>().AsSingle();
            Container.Bind<ISettingsInitializer>().To<SettingsInitializer>().AsSingle();
        }
    }
}