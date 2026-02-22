using System;
using Mechanics.Characters;

namespace Mechanics.Income
{
    public interface IGameEconomyService: IService
    {
        void Spend(float value);
        bool CanSpend(float value);
        void SpendClean(float value);
        bool CanSpendClean(float value);
        void SpendDirty(float value);
        bool CanSpendDirty(float value);
        void LaunderDirtyToClean(float amount);
        bool CanLaunder(float amount);
        float GetCleanBalance();
        float GetDirtyBalance();
        void AddCleanMoney(float amount);
        void AddDirtyMoney(float amount);
        float GetCompanyIncomePerMonth(string companyKey);
        float GetProductIncomePerMonth(string productKey);
        float GetCharacterIncomePerMonth(ICharacterData characterData);

        event Action<float> OnPlayerBalanceChanged;
        event Action<float> OnCleanBalanceChanged;
        event Action<float> OnDirtyBalanceChanged;
        event Action OnMyPlayerIncomeChanged;
        void ManualUpdateIncome(string characterKey);
    }
}