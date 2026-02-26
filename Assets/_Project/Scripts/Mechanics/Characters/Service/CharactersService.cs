using System.Threading.Tasks;
using Mechanics.Companies;
using Mechanics.Config;
using Mechanics.GameSave;
using Mechanics.Product;
using Zenject;

namespace Mechanics.Characters
{
    public class CharactersService : ICharactersService
    {
        private readonly ICharactersProvider _charactersProvider;
        private readonly ICompaniesProvider _companiesProvider;
        private readonly IGameSaveService _gameSaveService;
        private readonly DefaultPlayerConfig _defaultPlayerConfig;

        [Inject]
        public CharactersService(
            ICharactersProvider charactersProvider,
            ICompaniesProvider companiesProvider,
            IGameSaveService gameSaveService,
            DefaultPlayerConfig defaultPlayerConfig)
        {
            _charactersProvider = charactersProvider;
            _companiesProvider = companiesProvider;
            _gameSaveService = gameSaveService;
            _defaultPlayerConfig = defaultPlayerConfig;
        }

        public async Task Init()
        {
            if (_gameSaveService.IsGameLoaded())
                _charactersProvider.AddMyCharacter(_gameSaveService.MyCharacter);
            else
            {
                var config = _defaultPlayerConfig;
                var characterData = new CharacterData(config.Name, config.Age, config.Role, config.PeacefulRole,
                    config.Salary, string.Empty, config.Skill);
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