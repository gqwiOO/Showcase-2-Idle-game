namespace Core.Scripts.Services.UpdateService
{
    public interface IUpdateService
    {
        void Update();

        void Add(IUpdatable updatable);
        void Remove(IUpdatable updatable);
    }
}