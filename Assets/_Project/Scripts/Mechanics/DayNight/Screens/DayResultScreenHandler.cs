using Cysharp.Threading.Tasks;
using Services.Screen.Interfaces;
using UnityEngine;
using Zenject;

namespace Mechanics.DayNight
{
    public class DayResultScreenHandler : MonoBehaviour
    {
        private IGameTimeService _gameTimeService;
        private IMainScreenService _mainScreenService;

        [Inject]
        private void Construct(IGameTimeService gameTimeService, IMainScreenService mainScreenService)
        {
            _gameTimeService = gameTimeService;
            _mainScreenService = mainScreenService;
        }

        private void OnEnable()
        {
            _gameTimeService.OnDaySummaryReady += OnDaySummaryReady;
        }

        private void OnDisable()
        {
            _gameTimeService.OnDaySummaryReady -= OnDaySummaryReady;
        }

        private void OnDaySummaryReady(DaySummaryData data)
        {
            _mainScreenService.ShowDayResultScreen(data).Forget();
        }
    }
}
