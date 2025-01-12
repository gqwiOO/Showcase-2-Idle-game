using System.Collections.Generic;
using System.Threading.Tasks;
using Mechanics.Companies;
using Mechanics.CompaniesRating.Data;

namespace Mechanics.CompaniesRating.Generator
{
    public interface ICompaniesRatingGenerator
    {
        Task<List<ICompanyData>> GenerateCompanies(CompaniesGeneratingSettings settings);
    }
}