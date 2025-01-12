using System;
using System.Collections.Generic;
using Mechanics.Characters;

namespace Mechanics.Companies
{
    public interface ICompaniesService
    {
        ICompanyData CreateCompany(CompanyCreateData companyCreateData);
        void AddEmployeeToCompany(ICharacterData characterData, string companyKey);
        float GetCompanyEmployeesSalary(string companyKey);
        IEnumerable<ICharacterData> GetCompanyEmployees(string companyKey);
        event Action<ICompanyData> OnAnyCompanyUpdated;
    }
}