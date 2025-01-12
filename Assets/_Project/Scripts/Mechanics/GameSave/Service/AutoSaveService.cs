using System.Threading.Tasks;
using Core.Scripts.Services.UpdateService;
using Zenject;

namespace Mechanics.GameSave
{
    public class AutoSaveService : IAutoSaveService
    {
        private float _currentInterval;
        private IGameSaveService _saveService;
        private IUpdateService _updateService;
        public UpdateType UpdateType { get; }
        public float IntervalSeconds => 10f;

        [Inject]
        private void Construct(IGameSaveService saveService, IUpdateService updateService)
        {
            _updateService = updateService;
            _saveService = saveService;
        }
        
        public async Task Init()
        {
            _updateService.Add(this);
        }
        
        public void Tick(float tickTime)
        {
            _currentInterval += tickTime;
            
            if(_currentInterval > IntervalSeconds)
            {
                _currentInterval = 0;
                AutoSave();
            }
        }

        private void AutoSave()
        {
            _saveService.SaveGame();
        }
    }
}