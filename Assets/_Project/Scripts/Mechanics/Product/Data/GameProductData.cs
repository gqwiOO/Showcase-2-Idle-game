using System;
using Mechanics.Developing.Data;
using Newtonsoft.Json;

namespace Mechanics.Product
{
    
    [Serializable]
    public class GameProductData : IGameProductData
    {
        public event Action OnIncomeChanged;
        
        public event Action<IProductData> OnReleased;

        public string Key { get; set; }

        public string Name { get; set; }

        public float IncomePerMonth { get; set; }

        public ProductState ProductState { get; set; }

        [JsonIgnore]
        IDevelopingData IProductData.DevelopingData => _developingData;
        
        public DevelopingData _developingData { get; set; }

        public GameGenre Genre { get; set; }

        [JsonConstructor]
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
            Key = createGameProductData.Key;
            Name = createGameProductData.Name;
            IncomePerMonth = 5;
            Genre = createGameProductData.Genre;
            
            if (isReleased)
                ProductState = ProductState.Released;
            else
            {
                ProductState = ProductState.Developing;
                _developingData = createGameProductData.DevelopingData;
            }
        }

        public void ReleaseProduct()
        {
            ProductState = ProductState.Released;
            OnReleased?.Invoke(this);
        }
        
        public void SetNewIncome(float newIncome)
        {
            IncomePerMonth = newIncome;
            OnIncomeChanged?.Invoke();
        }
    }
}