using System;
using Mechanics.Income;
using Zenject;

namespace Mechanics.Income.Service
{
    public class LaunderService : ILaunderService
    {
        private readonly IGameEconomyService _gameEconomyService;

        public event Action OnMaxAmountChanged;

        [Zenject.Inject]
        public LaunderService(IGameEconomyService gameEconomyService)
        {
            _gameEconomyService = gameEconomyService;
            _gameEconomyService.OnDirtyBalanceChanged += RaiseMaxAmountChanged;
        }

        public float GetMaxLaunderAmount() => _gameEconomyService.GetDirtyBalance();

        public bool CanLaunder(float amount) => _gameEconomyService.CanLaunder(amount);

        public void Launder(float amount)
        {
            if (CanLaunder(amount))
                _gameEconomyService.LaunderDirtyToClean(amount);
        }

        private void RaiseMaxAmountChanged(float _) => OnMaxAmountChanged?.Invoke();
    }
}
