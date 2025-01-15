using System;
using Mechanics.Developing.Data;

namespace Mechanics.Product
{
    public interface IProductData: IIncomeObject
    {
        string Key { get; }
        string Name { get; }
        float IncomePerMonth { get; }
        ProductState ProductState { get; }
        IDevelopingData DevelopingData { get; }

        void SetNewIncome(float newIncome);

        event Action<IProductData> OnReleased;

        void ReleaseProduct();
    }

    public interface IIncomeObject
    {
        event Action OnIncomeChanged;
    }
}