using System.Collections.Generic;
using Mechanics.Characters;
using Zenject;

namespace Mechanics.Companies
{
    public class CompaniesProvider : ICompaniesProvider
    {
        private readonly Dictionary<string, ICompanyData> _companies = new();
        private ICharactersProvider _charactersProvider;
        
        [Inject]
        private void Construct(ICharactersProvider charactersProvider)
        {
            _charactersProvider = charactersProvider;
        }
        public ICompanyData GetCompanyByOwnerKey(string ownerKey)
        {
            var key = _charactersProvider.GetCharacterByKey(ownerKey).CompanyKey;
            _companies.TryGetValue(key, out var result);
            return result;
        }
        
        public ICompanyData GetCompanyByKey(string key)
        {
            _companies.TryGetValue(key, out var result);
            return result;
        }

        public ICompanyData GetMyCompany()
        {
            var key = _charactersProvider.GetMyCharacter().CompanyKey;
            return GetCompanyByKey(key);
        }

        public IEnumerable<ICompanyData> GetAllCompanies() 
            => _companies.Values;

        public void AddCompany(ICompanyData company)
        {
            _companies.TryAdd(company.Key, company);
        }
    }
}