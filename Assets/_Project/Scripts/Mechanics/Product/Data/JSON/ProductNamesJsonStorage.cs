using Core.Storage.JsonStorage;

namespace Mechanics.Product.JSON
{
    public class ProductNamesJsonStorage: BaseJsonResourcesStorage<GameNames>
    {
        public override string Path { get; } = "Data/Games/GameNames";
    }
}