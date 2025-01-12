using System.Collections.Generic;
using System.Linq;
using Mechanics.Characters;
using Sirenix.Utilities;
using Zenject;

namespace Mechanics.Product.Provider
{
    public interface IProductsProvider
    {
        IProductData GetProduct(string key);
        IEnumerable<IProductData> GetAllProductOfCharacter(string characterKey);
        void AddProduct(IProductData key);
        void RemoveProduct(string key);
    }
    
    public class ProductsProvider : IProductsProvider
    {
        private readonly Dictionary<string, IProductData> _products = new();
        private ICharactersProvider _charactersProvider;

        [Inject]
        private void Construct(ICharactersProvider charactersProvider)
        {
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