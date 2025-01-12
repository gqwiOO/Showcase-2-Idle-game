using System.Threading.Tasks;
using Mechanics.Companies;
using Mechanics.CompaniesRating.Data;
using Mechanics.CompaniesRating.Generator;
using Mechanics.DataSettings.Provider;
using Mechanics.Hiring.Data;
using Zenject;

namespace Mechanics.CompaniesRating.Service
{
    public class CompaniesRatingService : ICompaniesRatingService
    {
        private ISettingsProvider _settingsProvider;
        private ICompaniesProvider _companiesProvider;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider, ICompaniesProvider companiesProvider)
        {
            _companiesProvider = companiesProvider;
            _settingsProvider = settingsProvider;
        }
        
        public async Task Init()
        {
            var _companiesRatingGenerator = new CompaniesRatingGenerator();
            await _companiesRatingGenerator.Init(
                _settingsProvider.GetSettings<CompaniesGeneratingSettings>(),
                _settingsProvider.GetSettings<CharactersGeneratingSettings>(),
                _settingsProvider.GetSettings<ProductsGeneratingSettings>());

            foreach (var companyData in _companiesRatingGenerator.GeneratedCompanies)
            {
                _companiesProvider.AddCompany(companyData);
            }
        }
    }
}