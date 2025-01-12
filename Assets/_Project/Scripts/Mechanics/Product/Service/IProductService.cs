using System;
using Mechanics.Companies;

namespace Mechanics.Product
{
    public interface IProductService
    {
        event Action<IGameProductData> OnNewGameProductAdded;
        
        void CreateGameProduct(CreateGameProductData createGameProductData, string Owner);
        void CreateGameProduct(CreateGameProductData createGameProductData, ICompanyData Owner);
        string GetRandomGameNameWithGenre(GameGenre gameGenre, string currentName = "");
    }
}