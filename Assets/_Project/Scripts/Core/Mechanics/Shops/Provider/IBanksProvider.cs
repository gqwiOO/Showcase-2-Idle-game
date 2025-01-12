using System.Collections.Generic;
using Core.Storage.Bank;

namespace Core.Mechanics.Shops
{
    public interface IBanksProvider<TData>
    {
        void AddBank(BankId bankId,IDataBank<TData> dataBank);
        IDataBank<TData> Get(BankId bankId);
    }

    public class BanksProvider<TData> : IBanksProvider<TData>
    {
        private Dictionary<BankId, IDataBank<TData>> _banks = new Dictionary<BankId, IDataBank<TData>>();
        
        public void AddBank(BankId bankId, IDataBank<TData> dataBank)
        {
            _banks.Add(bankId,dataBank);
        }
        
        public IDataBank<TData> Get(BankId bankId)
        {
            return _banks[bankId];
        }
    }
}