namespace Core.Scripts.Services.UpdateService
{
    public interface IUpdatable
    {
        UpdateType UpdateType { get; }
        void Tick(float tickTime);
    }
}