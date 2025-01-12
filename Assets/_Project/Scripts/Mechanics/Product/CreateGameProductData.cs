using Mechanics.Developing.Data;

namespace Mechanics.Product
{
    public class CreateGameProductData
    {
        public string Name { get; private set; }
        public string key { get; private set; }
        
        public GameGenre Genre { get; private set; }
        public ProductState ProductState { get; private set; }
        public IDevelopingData DevelopingData { get; private set; }

        public CreateGameProductData(string name, GameGenre genre, IDevelopingData developingData, string key, ProductState productState = ProductState.Developing)
        {
            Name = name;
            Genre = genre;
            DevelopingData = developingData;
            this.key = key;
            ProductState = productState;
        }
    }
}