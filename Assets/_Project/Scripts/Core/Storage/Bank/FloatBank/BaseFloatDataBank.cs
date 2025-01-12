using System;
using Core.Mechanics.Shops;

namespace Core.Storage.Bank.FloatBank
{
    public class BaseFloatDataBank : IDataBank<float>
    {
        public BankId Key => BankId.None;
        public float Value { get; private set; }
        public float MaxValue { get; private set;}
        public float MinValue { get; private set;}
        public event EventHandler<DataBankEvent<float>> OnMax;
        public event EventHandler<DataBankEvent<float>> OnMin;
        public event EventHandler<DataBankEvent<float>> OnChanged;
        public event EventHandler<DataBankEvent<float>> OnAdded;
        
        public BaseFloatDataBank(float startValue = 0) => Value = startValue;

        public virtual void Add(float addValue)
        {
            Value += addValue;
            OnChanged?.Invoke(this,StorageTool.CreateEventData(this));
        }
        public virtual void Spend(float addValue)
        {
            Value -= addValue;
            OnChanged?.Invoke(this,StorageTool.CreateEventData(this));
        }

        public bool CanSpend(float value) => Value >= value;
        public void SetValue(float value)
        {
            Value = value;
            OnChanged?.Invoke(this,StorageTool.CreateEventData(this));
        }

        public float GetValue() => Value;
    }
}