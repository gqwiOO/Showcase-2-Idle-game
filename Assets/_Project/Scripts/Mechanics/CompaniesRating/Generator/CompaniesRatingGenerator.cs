using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extentions;
using Core.Scripts.Extension.System;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.CompaniesRating.Data;
using Mechanics.CompaniesRating.Json;
using Mechanics.Hiring.Data;
using Mechanics.Product;
using Random = System.Random;

namespace Mechanics.CompaniesRating.Generator
{
    public class CompaniesRatingGenerator : ICompaniesRatingGenerator
    {
        private CompaniesGeneratingSettingsJsonStorage _companiesGeneratingSettingsJsonStorage;
        private CompaniesGeneratingSettings _settings;
        private CharactersGeneratingSettings _charactersGeneratingSettings;
        private ProductsGeneratingSettings _productsGeneratingSettings;
        private List<ICompanyData> _companies;

        public IEnumerable<ICompanyData> GeneratedCompanies => _companies;

        public async Task Init(CompaniesGeneratingSettings companiesGeneratingSettings,CharactersGeneratingSettings charactersGeneratingSettings, ProductsGeneratingSettings productsGeneratingSettings)
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
            
            foreach (var product in products)
                company.AddProduct(product.Key);

            owner.SetCompanyKey(company.Key);
            companies.Add(company);

            return company;
        }

        private List<ICharacterData> GenerateCompanyEmployees(CompanySettingsByTier settingsByTier, string companyKey, List<string> charactersNames)
        {
            var result = new List<ICharacterData>();

            var employeesCount = new System.Random().Next((int)settingsByTier.EmployeesCount.x,(int)settingsByTier.EmployeesCount.y);
            for (int i = 0; i < employeesCount; i++)
            {
                // TODO : employees skill should not be always expert 
                
                CharacterSkill skill = CharacterSkill.Expert;
                int employeeSalary = new Random().Next((int)settingsByTier.EmployeeSalary.x,(int)settingsByTier.EmployeeSalary.y);
                
                ICharacterData characterData = new CharacterData(
                    ListExtension.PickRandom(charactersNames),
                    _charactersGeneratingSettings.GetAge(skill)
                    ,RoleType.Developer,
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
            ICharacterData characterData = new CharacterData(
                ListExtension.PickRandom(names),
                _charactersGeneratingSettings.GetAge(skill)
                ,RoleType.Developer,
                _charactersGeneratingSettings.GetSalary(skill),
                "",
                skill);

            return characterData;
        }

        private async Task<List<IProductData>> GenerateCompanyProducts(CompanySettingsByTier settings, ProductsGeneratingSettings productsGeneratingSettings)
        {
            var result = new List<IProductData>();

            int productsCount = new Random().Next((int)settings.ProductsCountRange.x, (int)settings.ProductsCountRange.y);

            var random = new Random();

            var tasks = new List<Task>(productsCount);
            for (int i = 0; i < productsCount; i++)
            {
                tasks.Add(CreateProduct(productsGeneratingSettings, random, result));
            }

            await Task.WhenAll(tasks);

            return result;
        }

        private Task CreateProduct(ProductsGeneratingSettings productsGeneratingSettings, Random random, List<IProductData> result)
        {
            var genre = random.GetRandomValueExceptFirst<GameGenre>();
            var name = productsGeneratingSettings
                .ProductsSettingsByGenres
                .First(item => item.Genre == genre)
                .Names
                .PickRandom();
                
            var createProductData = new CreateGameProductData(name,genre,null,Guid.NewGuid().ToString(),ProductState.Released);
            var product = new GameProductData(createProductData, true);
            result.Add(product);

            return Task.CompletedTask;
        }
    }
}