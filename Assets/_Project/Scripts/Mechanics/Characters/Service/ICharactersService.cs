using Mechanics.Companies;
using Mechanics.Product;

namespace Mechanics.Characters
{
    public interface ICharactersService : IService, IGameSaveDependent
    {
        void AddContractToCharacter(string characterKey, IContractData contractData);
        void AddContractToCharacter(ICharacterData character, IContractData contractData);
        void AddContractToCompany(ICompanyData company, IContractData contractData);
        void AddContractToCompany(string companyKey, IContractData contractData);
    }
}