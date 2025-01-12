using System;
using Mechanics.Developing.Data;

namespace Mechanics.Product
{
    public class GameProductData : IGameProductData
    {
        public event Action OnIncomeChanged;
        public event Action<IProductData> OnReleased;

        public string Key { get; set; }

        public string Name { get; set; }

        public float IncomePerMonth { get; set; }

        public ProductState ProductState { get; set; }

        public IDevelopingData DevelopingData { get; set; }

        public GameGenre Genre { get; set; }

        public GameProductData(string key, string name, float income, GameGenre genre, ProductState productState = ProductState.Developing)
        {
            Key = key;
            Name = name;
            IncomePerMonth = income;
            Genre = genre;
            ProductState = productState;
        }

        public GameProductData(CreateGameProductData createGameProductData, bool isReleased = false)
        {
            Key = createGameProductData.key;
            Name = createGameProductData.Name;
            IncomePerMonth = 5;
            Genre = createGameProductData.Genre;
            
            if (isReleased)
                ProductState = ProductState.Released;
            else
            {
                ProductState = ProductState.Developing;
                DevelopingData = createGameProductData.DevelopingData;
            }
        }

        public void ReleaseProduct()
        {
            ProductState = ProductState.Released;
            OnReleased?.Invoke(this);
        }
    }
}