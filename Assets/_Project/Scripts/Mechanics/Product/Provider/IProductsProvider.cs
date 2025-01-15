using System.Collections.Generic;
using System.Linq;
using Mechanics.Characters;
using Mechanics.Companies;
using Sirenix.Utilities;
using Zenject;

namespace Mechanics.Product.Provider
{
    public interface IProductsProvider
    {
        IProductData GetProduct(string key);
        IEnumerable<IProductData> GetAllProductOfCharacter(string characterKey);
        IReadOnlyList<IProductData> GetAllProductOfCompany(string companyKey);
        IEnumerable<IProductData> GetAllProduct();
        void AddProduct(IProductData key);
        void RemoveProduct(string key);
    }
    
    public class ProductsProvider : IProductsProvider
    {
        private readonly Dictionary<string, IProductData> _products = new();
        private ICharactersProvider _charactersProvider;
        private ICompaniesProvider _companiesProvider;

        [Inject]
        private void Construct(ICharactersProvider charactersProvider, ICompaniesProvider companiesProvider)
        {
            _companiesProvider = companiesProvider;
            _charactersProvider = charactersProvider;
        }
        

        public IProductData GetProduct(string key)
        {
            _products.TryGetValue(key, out var product);
            return product;
        }

        public IEnumerable<IProductData> GetAllProductOfCharacter(string characterKey)
        {
            var character = _charactersProvider.GetCharacterByKey(characterKey);
            var productsKeys = character.Products;

            List<IProductData> result = new(productsKeys.Count());
            
            productsKeys.ForEach(key =>
            {
                _products.TryGetValue(key, out var product);
                result.Add(product);
            });

            return result;
        }

        public IReadOnlyList<IProductData> GetAllProductOfCompany(string companyKey)
        {
            var company = _companiesProvider.GetCompanyByKey(companyKey);
            var productsKeys = new List<string>(company.Products);
            
            var ownerProductsKeys = _charactersProvider.GetCharacterByKey(company.Owner).Products;
            productsKeys.AddRange(ownerProductsKeys);

            List<IProductData> result = new(productsKeys.Count());
            
            foreach (var productsKey in productsKeys)
            {
                _products.TryGetValue(productsKey, out var product);
                result.Add(product);
            }
            return result;
        }

        public IEnumerable<IProductData> GetAllProduct() => _products.Values;

        public void AddProduct(IProductData product)
        {
            _products.TryAdd(product.Key, product);
        }

        public void RemoveProduct(string key)
        {
            _products.Remove(key);
        }
    }
}