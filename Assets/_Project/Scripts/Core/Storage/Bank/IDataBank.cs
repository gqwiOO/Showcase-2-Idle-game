using System;
using Core.Mechanics.Shops.Provider;

namespace Core.Storage.Bank
{
    public interface IDataBank<TData>
    {
        BankId Key { get; }
        TData Value { get; }
        TData MaxValue { get; }
        TData MinValue { get; }

        event EventHandler<DataBankEvent<TData>> OnMax;
        event EventHandler<DataBankEvent<TData>> OnMin;
        event EventHandler<DataBankEvent<TData>> OnChanged;
        event EventHandler<DataBankEvent<TData>> OnAdded;

        void Add(TData value);
        void Spend(TData value);
        bool CanSpend(TData value);
        TData GetValue();
    }
    public interface IDataBank
    {
        object Value { get; }
        object MaxValue { get; }
        object MinValue { get; }
    }
    
}