using Mechanics.Companies;
using Mechanics.Product;

namespace Mechanics.Characters
{
    public interface ICharactersService : IService
    {
        void AddProductToCharacter(string characterKey, IProductData productData);
        void AddProductToCharacter(ICharacterData character, IProductData productData);
        void AddProductToCompany(ICompanyData company, IProductData productData);
        void AddProductToCompany(string companyKey, IProductData productData);
    }
}