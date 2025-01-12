using Core.Scripts.Services.UpdateService;

namespace Mechanics.GameSave
{
    public interface IAutoSaveService: IService, IUpdatable
    {
        public float IntervalSeconds { get; }
    }
}