using System.Collections.Generic;

namespace Mechanics.Companies
{
    public interface ICompaniesProvider
    {
        ICompanyData GetCompanyByOwnerKey(string ownerKey);
        ICompanyData GetCompanyByKey(string key);
        ICompanyData GetMyCompany();
        IEnumerable<ICompanyData> GetAllCompanies();
        void AddCompany(ICompanyData company);
    }
}