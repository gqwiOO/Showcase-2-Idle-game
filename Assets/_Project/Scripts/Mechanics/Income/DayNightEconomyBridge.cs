using System;
using Mechanics.DayNight;
using Zenject;

namespace Mechanics.Income
{
    /// <summary>
    /// Connects economy and day/night without circular dependency:
    /// forwards balance changes to the time service and sets day-start balance on phase change.
    /// </summary>
    public class DayNightEconomyBridge : IInitializable, IDisposable
    {
        private readonly IGameEconomyService _gameEconomyService;
        private readonly IGameTimeService _gameTimeService;
        private readonly IGameTimeBalanceInput _gameTimeBalanceInput;

        [Inject]
        public DayNightEconomyBridge(
            IGameEconomyService gameEconomyService,
            IGameTimeService gameTimeService,
            IGameTimeBalanceInput gameTimeBalanceInput)
        {
            _gameEconomyService = gameEconomyService;
            _gameTimeService = gameTimeService;
            _gameTimeBalanceInput = gameTimeBalanceInput;
        }

        public void Initialize()
        {
            _gameEconomyService.OnPlayerBalanceChanged += OnPlayerBalanceChanged;
            _gameTimeService.OnPhaseChanged += OnPhaseChanged;
            if (_gameTimeService.CurrentPhase == DayNightPhase.Day)
                _gameTimeBalanceInput.NotifyDayStarted(_gameEconomyService.GetCleanBalance());
        }

        public void Dispose()
        {
            _gameEconomyService.OnPlayerBalanceChanged -= OnPlayerBalanceChanged;
            _gameTimeService.OnPhaseChanged -= OnPhaseChanged;
        }

        private void OnPlayerBalanceChanged(float balance)
        {
            _gameTimeBalanceInput.NotifyBalanceChanged(balance);
        }

        private void OnPhaseChanged(DayNightPhase phase)
        {
            if (phase == DayNightPhase.Day)
                _gameTimeBalanceInput.NotifyDayStarted(_gameEconomyService.GetCleanBalance());
        }
    }
}
