using Zenject;

namespace Mechanics.Characters
{
    public class CharactersInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CharactersProvider>().AsSingle();
            Container.BindInterfacesTo<CharactersService>().AsSingle();
        }
    }
}