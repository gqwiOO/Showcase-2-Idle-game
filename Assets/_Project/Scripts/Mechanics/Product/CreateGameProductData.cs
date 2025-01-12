using Mechanics.Developing.Data;

namespace Mechanics.Product
{
    public class CreateGameProductData
    {
        public string Name { get; private set; }
        public string Key { get; private set; }
        
        public GameGenre Genre { get; private set; }
        public ProductState ProductState { get; private set; }
        public DevelopingData DevelopingData { get; private set; }
        
        public void SetKey(string key) => Key = key;

        public CreateGameProductData(string name, GameGenre genre, DevelopingData developingData, string key, ProductState productState = ProductState.Developing)
        {
            Name = name;
            Genre = genre;
            DevelopingData = developingData;
            this.Key = key;
            ProductState = productState;
        }
    }
}