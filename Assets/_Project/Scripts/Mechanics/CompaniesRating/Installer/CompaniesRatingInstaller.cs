using Mechanics.CompaniesRating.Service;
using Zenject;

namespace Mechanics.CompaniesRating.Installer
{
    public class CompaniesRatingInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ICompaniesRatingService>().To<CompaniesRatingService>().AsSingle();
        }
    }
}