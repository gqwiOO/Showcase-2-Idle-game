using Mechanics.CompaniesRating.Generator;
using Mechanics.CompaniesRating.Service;
using Zenject;

namespace Mechanics.CompaniesRating.Installer
{
    public class CompaniesRatingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ICompaniesRatingGenerator>().To<CompaniesRatingGenerator>().AsSingle();
            Container.Bind<ICompaniesRatingService>().To<CompaniesRatingService>().AsSingle();
        }
    }
}