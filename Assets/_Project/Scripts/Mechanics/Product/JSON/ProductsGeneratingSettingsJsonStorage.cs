using Core.Storage.JsonStorage;
using Mechanics.CompaniesRating.Generator;

namespace Mechanics.Product.JSON
{
    public class ProductsGeneratingSettingsJsonStorage: BaseJsonStorage<ProductsGeneratingSettings>
    {
        public override string Path => "Data/Settings/ProductsGeneratingSettings";
    }
}