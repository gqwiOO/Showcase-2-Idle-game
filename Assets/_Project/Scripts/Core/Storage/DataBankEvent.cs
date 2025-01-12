using System;

namespace Core.Storage
{
    [Serializable]
    public class DataBankEvent<TData>: EventArgs
    {
        public TData CurrentValue;

        public DataBankEvent(TData currentValue)
        {
            CurrentValue = currentValue;
        }
    }
}