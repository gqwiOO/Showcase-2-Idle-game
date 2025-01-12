using Core.Storage.Bank;

namespace Core.Storage
{
    public static class StorageTool
    {
        public static DataBankEvent<TData> CreateEventData<TData>(IDataBank<TData> dataBank)
            => new(dataBank.Value);
    }
}