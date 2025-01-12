using Core.Storage.Bank.FloatBank;
using Core.Storage.Bank.IntBank;
using Zenject;

namespace Core.Mechanics.Shops.Provider
{
    public class BanksFactory: IBanksFactory, IInitializable
    {
        
        private IBanksProvider<int> _banksIntProvider;
        private IBanksProvider<float> _banksFloatProvider;
        
        [Inject]
        private void Construct(IBanksProvidersProvider scoreBanksProvider)
        {
            _banksIntProvider = scoreBanksProvider.GetIntBankProvider();
            _banksFloatProvider = scoreBanksProvider.GetFloatBankProvider();
            CreateFloatBankWithId(BankId.FruitsBank,500);


        }
        public void Initialize()
        {       
        }
        
        public BaseIntDataBank CreateIntBankWithId(BankId bankId, int startValue)
        {
            var bank = new BaseIntDataBank(startValue);
            _banksIntProvider.AddBank(bankId, bank);
            return bank;
        }
        
        public BaseFloatDataBank CreateFloatBankWithId(BankId bankId, float startValue)
        {
            var bank = new BaseFloatDataBank(startValue);
            _banksFloatProvider.AddBank(bankId, bank);
            return bank;
        }
    }
}