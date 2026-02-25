using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.GameSave;
using Zenject;

namespace Mechanics.Companies
{
    public class CompaniesService : ICompaniesService
    {
        private ICompaniesProvider _companiesProvider;
        private ICharactersProvider _charactersProvider;
        private IGameSaveService _gameSaveService;

        private const string DefaultCompanyName = "Hotel";

        public event Action<ICompanyData> OnAnyCompanyUpdated;

        [Inject]
        private void Construct(ICompaniesProvider companiesProvider, ICharactersProvider charactersProvider,
            IGameSaveService gameSaveService)
        {
            _charactersProvider = charactersProvider;
            _companiesProvider = companiesProvider;
            _gameSaveService = gameSaveService;
        }

        public async Task Init()
        {
            if (!_gameSaveService.IsGameLoaded())
            {
                var myCharacter = _charactersProvider.GetMyCharacter();
                if (myCharacter != null)
                    CreateCompany(new CompanyCreateData(DefaultCompanyName, myCharacter.Key));
            }
        }

        public void InjectGameSave(GameData gameSave)
        {
            foreach (var gameSaveCompany in gameSave.Companies)
            {
                _companiesProvider.AddCompany(gameSaveCompany);
            }
        }

        public ICompanyData CreateCompany(CompanyCreateData companyCreateData)
        {
            ICharacterData owner = _charactersProvider.GetCharacterByKey(companyCreateData.OwnerKey);
            var result = new CompanyData(owner.GetHashCode().ToString() ,companyCreateData.Name,companyCreateData.OwnerKey);

            owner.SetCompanyKey(result.Key);
            result.AddEmployee(owner.Key);
            
            _companiesProvider.AddCompany(result);

            OnAnyCompanyUpdated?.Invoke(result);
            return result;
        }

        public void AddEmployeeToCompany(ICharacterData characterData, string companyKey)
        {
            var companyData = _companiesProvider.GetCompanyByKey(companyKey);
            if (companyData == null)
                return;
            
            companyData.AddEmployee(characterData.Key);
            OnAnyCompanyUpdated?.Invoke(companyData);
        }

        public IEnumerable<ICharacterData> GetCompanyEmployees(string companyKey)
        {
            var company = _companiesProvider.GetCompanyByKey(companyKey);
            if (company == null)
                return new List<ICharacterData>();
            
            return company.Employees.Select(item => _charactersProvider.GetCharacterByKey(item));
        }

        public float GetCompanyEmployeesSalary(string companyKey)
        {
            var totalSalaries = 0f;
            var company = _companiesProvider.GetCompanyByKey(companyKey);
            if (company == null)
                return totalSalaries;
            
            IEnumerable<ICharacterData> employees = company.Employees.Select(item => _charactersProvider.GetCharacterByKey(item));

            foreach (var employee in employees)
                totalSalaries += employee.Salary;

            return totalSalaries;
        }
    }
}