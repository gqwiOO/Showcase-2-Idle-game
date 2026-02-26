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
                var characterData = new CharacterData("l2fx6", 18, RoleType.Handler, PeacefulRoleType.Manager, 0,
                    string.Empty, CharacterSkill.Expert);
                _charactersProvider.AddMyCharacter(characterData);
            }
        }

        public void InjectGameSave(GameData gameSave)
        {
            var myCharacterKey = gameSave.MyCharacter?.Key;
            foreach (var gameSaveCharacter in gameSave.Characters)
            {
                if (gameSaveCharacter.Key == myCharacterKey)
                    continue;
                _charactersProvider.AddCharacter(gameSaveCharacter);
            }
        }

        public void AddContractToCharacter(string characterKey, IContractData contractData)
        {
            var character = _charactersProvider.GetCharacterByKey(characterKey);
            AddContractToCharacter(character, contractData);
        }

        public void AddContractToCharacter(ICharacterData character, IContractData contractData)
            => character.AddContract(contractData.Key);

        public void AddContractToCompany(ICompanyData company, IContractData contractData)
            => company.AddContract(contractData.Key);

        public void AddContractToCompany(string companyKey, IContractData contractData)
        {
            var company = _companiesProvider.GetCompanyByOwnerKey(companyKey);
            AddContractToCompany(company, contractData);
        }
    }
}