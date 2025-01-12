using System;
using Core.Mechanics.Shops;
using Core.Scripts.Debugging;

namespace Core.Storage.Bank.IntBank
{
    public class BaseIntDataBank : IDataBank<int>
    {
        public BankId Key => BankId.None;
        public int IDataBankValue { get; private set; }
        public int Value { get;  private set;}
        public int MaxValue { get; private set; } = int.MaxValue;
        public int MinValue { get;  private set;} = int.MaxValue;
        public event EventHandler<DataBankEvent<int>> OnMax;
        public event EventHandler<DataBankEvent<int>> OnMin;
        public event EventHandler<DataBankEvent<int>> OnChanged;
        public event EventHandler<DataBankEvent<int>> OnAdded;
        
        public BaseIntDataBank(int startValue = 0) => Value = startValue;
        public virtual void Add(int addValue)
        {
            Value += addValue;
            Debugging.Log(this,$"Added {addValue}");
            OnChanged?.Invoke(this,StorageTool.CreateEventData(this));
        }
        public virtual void Spend(int addValue)
        {
            Value -= addValue;
            OnChanged?.Invoke(this,StorageTool.CreateEventData(this));
        }

        public bool CanSpend(int value) => Value >= value;
        public void SetValue(int value)
        {
            Value = value;
            OnChanged?.Invoke(this,StorageTool.CreateEventData(this));
        }

        public int GetValue() 
            => Value;
    }
}