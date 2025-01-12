using Core.Storage.Bank.FloatBank;
using Core.Storage.Bank.IntBank;

namespace Core.Mechanics.Shops.Provider
{
    public interface IBanksFactory
    {
        public BaseIntDataBank CreateIntBankWithId(BankId bankId, int startValue);
        public BaseFloatDataBank CreateFloatBankWithId(BankId bankId, float startValue);
    }
}