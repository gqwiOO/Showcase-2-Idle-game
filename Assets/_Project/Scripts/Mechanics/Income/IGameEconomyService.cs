using System;
using Mechanics.Characters;

namespace Mechanics.Income
{
    public interface IGameEconomyService: IService
    {
        void Spend(float value);
        bool CanSpend(float value);
        float GetCompanyIncomePerMonth(string companyKey);
        float GetProductIncomePerMonth(string productKey);
        float GetCharacterIncomePerMonth(ICharacterData characterData);

        event Action<float> OnPlayerBalanceChanged;
        event Action OnMyPlayerIncomeChanged;
    }
}