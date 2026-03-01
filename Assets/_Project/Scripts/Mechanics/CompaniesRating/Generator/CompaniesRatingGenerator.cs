using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extentions;
using Core.Scripts.Extension.System;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.CompaniesRating.Data;
using Mechanics.Config;
using Mechanics.DataSettings;
using Mechanics.Hiring.Data;
using Mechanics.Product;

namespace Mechanics.CompaniesRating.Generator
{
    public class CompaniesRatingGenerator : ICompaniesRatingGenerator
    {
        private CompaniesGeneratingSettings _settings;
        private CharactersGeneratingSettings _charactersGeneratingSettings;
        private ProductsGeneratingSettings _productsGeneratingSettings;
        private readonly ContractProductLimitsConfig _productLimitsConfig;
        private List<ICompanyData> _companies;

        public IEnumerable<ICompanyData> GeneratedCompanies => _companies;

        [Zenject.Inject]
        public CompaniesRatingGenerator(ContractProductLimitsConfig productLimitsConfig)
        {
            _productLimitsConfig = productLimitsConfig;
        }

        public async Task Init(CompaniesGeneratingSettings companiesGeneratingSettings, CharactersGeneratingSettings charactersGeneratingSettings, ProductsGeneratingSettings productsGeneratingSettings)
        {
            _settings = companiesGeneratingSettings;
            _productsGeneratingSettings = productsGeneratingSettings;
            _charactersGeneratingSettings = charactersGeneratingSettings;
            _companies = await GenerateCompanies(_settings);
        }
        
        public async Task<List<ICompanyData>> GenerateCompanies(CompaniesGeneratingSettings settings)
        {
            var companies = new List<ICompanyData>();

            List<Task> tasks = new List<Task>(settings.GeneratedCount);

            for (int C = 0; C < settings.GeneratedCount; C++)
                tasks.Add(GenerateCompany(settings, companies));
            
            await Task.WhenAll(tasks);
            return await Task.FromResult(companies);
        }

        private async Task<ICompanyData> GenerateCompany(CompaniesGeneratingSettings settings, List<ICompanyData> companies)
        {
            var name = GenerateCompanyName(settings.Names);
            var owner = GenerateCompanyOwner(_charactersGeneratingSettings.Names);
            var company = new CompanyData(name, owner.Key);

            var randomTier = (CompanyTier)(new System.Random().Next(1, Enum.GetNames(typeof(CompanyTier)).Length));
            var employees = GenerateCompanyEmployees(
                settings.CachedCompanySettingsByTier[randomTier],
                company.Key,
                _charactersGeneratingSettings.Names
            );
            
            var products =  await GenerateCompanyProducts(
                settings.CachedCompanySettingsByTier[randomTier],
                _productsGeneratingSettings
                );

            foreach (var characterData in employees)
                company.AddEmployee(characterData.Key);
            
            foreach (var contract in products)
                company.AddContract(contract.Key);

            owner.SetCompanyKey(company.Key);
            companies.Add(company);

            return company;
        }

        private List<ICharacterData> GenerateCompanyEmployees(CompanySettingsByTier settingsByTier, string companyKey, List<string> charactersNames)
        {
            var result = new List<ICharacterData>();

            var employeesCount = settingsByTier.EmployeesCount.Value;
            for (int i = 0; i < employeesCount; i++)
            {
                // TODO : employees skill should not be always expert 
                
                CharacterSkill skill = CharacterSkill.Expert;
                int employeeSalary = settingsByTier.EmployeeSalary.Value;
                
                var blackRole = (RoleType)new Random().Next(1, Enum.GetNames(typeof(RoleType)).Length);
                var peacefulRole = (PeacefulRoleType)new Random().Next(1, Enum.GetNames(typeof(PeacefulRoleType)).Length);
                ICharacterData characterData = new CharacterData(
                    ListExtension.PickRandom(charactersNames),
                    _charactersGeneratingSettings.GetAge(skill),
                    blackRole,
                    peacefulRole,
                    employeeSalary,
                    companyKey,
                    skill);

                result.Add(characterData);
            }
            return result;
        }

        private string GenerateCompanyName(List<string> settingsNames) 
            => ListExtension.PickRandom(settingsNames);
        
        private ICharacterData GenerateCompanyOwner(List<string> names)
        {

            CharacterSkill skill = CharacterSkill.Expert;
            var blackRole = (RoleType)new Random().Next(1, Enum.GetNames(typeof(RoleType)).Length);
            var peacefulRole = (PeacefulRoleType)new Random().Next(1, Enum.GetNames(typeof(PeacefulRoleType)).Length);
            ICharacterData characterData = new CharacterData(
                ListExtension.PickRandom(names),
                _charactersGeneratingSettings.GetAge(skill),
                blackRole,
                peacefulRole,
                _charactersGeneratingSettings.GetSalary(skill),
                "",
                skill);

            return characterData;
        }

        private async Task<List<IContractData>> GenerateCompanyProducts(CompanySettingsByTier settings, ProductsGeneratingSettings productsGeneratingSettings)
        {
            var result = new List<IContractData>();

            int contractsCount = settings.ProductsCountRange.Value;

            var random = new Random();

            var tasks = new List<Task>(contractsCount);
            for (int i = 0; i < contractsCount; i++)
            {
                tasks.Add(CreateContract(productsGeneratingSettings, random, result));
            }

            await Task.WhenAll(tasks);

            return result;
        }

        private Task CreateContract(ProductsGeneratingSettings productsGeneratingSettings, Random random, List<IContractData> result)
        {
            var genre = random.GetRandomValueExceptFirst<GameGenre>();
            var name = productsGeneratingSettings
                .ProductsSettingsByGenres
                .First(item => item.Genre == genre)
                .Names
                .PickRandom();

            var contract = ContractData.Create(Guid.NewGuid().ToString(), name, null,
                _productLimitsConfig.DefaultRewardAmount, _productLimitsConfig.DefaultReputationReward, _productLimitsConfig.DefaultHeatReward);
            contract.ContractState = ContractState.Completed;
            result.Add(contract);

            return Task.CompletedTask;
        }
    }

    [Serializable]
    public class ProductsGeneratingSettings: ISettingsData
    {
        public List<ProductsSettingsByGenre> ProductsSettingsByGenres;
    }

    [Serializable]
    public class ProductsSettingsByGenre
    {
        public GameGenre Genre;
        public List<string> Names;
    }
}