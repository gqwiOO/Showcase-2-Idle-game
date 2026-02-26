using System.Threading.Tasks;
using Mechanics.Companies;
using Mechanics.CompaniesRating.Data;
using Mechanics.CompaniesRating.Generator;
using Mechanics.Hiring.Data;
using Zenject;

namespace Mechanics.CompaniesRating.Service
{
    public class CompaniesRatingService : ICompaniesRatingService
    {
        private readonly ICompaniesProvider _companiesProvider;
        private readonly ICompaniesRatingGenerator _generator;
        private readonly CharactersGeneratingSettings _charactersSettings;

        [Inject]
        public CompaniesRatingService(
            ICompaniesProvider companiesProvider,
            ICompaniesRatingGenerator generator,
            CharactersGeneratingSettings charactersSettings)
        {
            _companiesProvider = companiesProvider;
            _generator = generator;
            _charactersSettings = charactersSettings;
        }

        public async Task Init()
        {
        }
    }
}