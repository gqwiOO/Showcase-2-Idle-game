using System;
using System.Collections.Generic;
using Mechanics.Characters;
using Mechanics.GameSave;

namespace Mechanics.Companies
{
    public interface ICompaniesService: IService, IGameSaveDependent
    {
        ICompanyData CreateCompany(CompanyCreateData companyCreateData);
        void AddEmployeeToCompany(ICharacterData characterData, string companyKey);
        float GetCompanyEmployeesSalary(string companyKey);
        IEnumerable<ICharacterData> GetCompanyEmployees(string companyKey);
        event Action<ICompanyData> OnAnyCompanyUpdated;
    }

    public interface IGameSaveDependent
    {
        void InjectGameSave(GameData gameSave);
    }
}