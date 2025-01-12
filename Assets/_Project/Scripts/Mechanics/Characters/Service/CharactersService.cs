using System.Threading.Tasks;
using Mechanics.Companies;
using Mechanics.GameSave;
using Mechanics.Product;
using Zenject;

namespace Mechanics.Characters
{
    public class CharactersService : ICharactersService
    {
        private ICharactersProvider _charactersProvider;
        private ICompaniesProvider _companiesProvider;

        private IGameSaveService _gameSaveService;

        [Inject]
        private void Construct(ICharactersProvider charactersProvider, ICompaniesProvider companiesProvider, IGameSaveService gameSaveService)
        {
            _gameSaveService = gameSaveService;
            _companiesProvider = companiesProvider;
            _charactersProvider = charactersProvider;
        }

        public async Task Init()
        {
            if(_gameSaveService.IsGameLoaded())
                _charactersProvider.AddMyCharacter(_gameSaveService.MyCharacter);
            else
            {
                var characterData = new CharacterData("l2fx6", 18, RoleType.Developer, 0, string.Empty,
                    CharacterSkill.Expert);
                _charactersProvider.AddMyCharacter(characterData);
            }
        }

        public void InjectGameSave(GameData gameSave)
        {
            foreach (var gameSaveCharacter in gameSave.Characters)
            {
                _charactersProvider.AddCharacter(gameSaveCharacter);
            }
        }

        public void AddProductToCharacter(string characterKey, IProductData productData)
        {
            var character = _charactersProvider.GetCharacterByKey(characterKey);
            AddProductToCharacter(character,productData);
        }

        public void AddProductToCharacter(ICharacterData character, IProductData productData) 
            => character.AddProduct(productData.Key);

        public void AddProductToCompany(ICompanyData company, IProductData productData) 
            => company.AddProduct(productData.Key);

        public void AddProductToCompany(string companyKey, IProductData productData)
        {
            var company = _companiesProvider.GetCompanyByOwnerKey(companyKey);
            AddProductToCompany(company, productData);
        }
    }
}