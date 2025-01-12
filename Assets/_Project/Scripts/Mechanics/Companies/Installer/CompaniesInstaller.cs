using Zenject;

namespace Mechanics.Companies
{
    public class CompaniesInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CompaniesService>().AsSingle();
            Container.BindInterfacesTo<CompaniesProvider>().AsSingle();
            Container.BindInterfacesTo<CharacterCompanyListener>().AsSingle();
        }
    }
}