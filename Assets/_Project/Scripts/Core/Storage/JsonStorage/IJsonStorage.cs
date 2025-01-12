namespace Core.Storage.JsonStorage
{
    public interface IJsonStorage<TData>
    {
        string Path { get; }
        
        void Load();
        
        TData Get();
        
        void Update(TData data);
        
        void Save();
    }
}