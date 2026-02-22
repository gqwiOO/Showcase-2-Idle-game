using Mechanics.Income;
using UnityEngine;
using Zenject;

namespace Mechanics.DayNight
{
    /// <summary>
    /// Must be registered before EconomyInstaller (GameEconomyService depends on IGameTimeService).
    /// </summary>
    public class GameTimeInstaller : MonoInstaller
    {
        [SerializeField] private DayNightConfig _config;

        public override void InstallBindings()
        {
            Container.Bind<DayNightConfig>().FromInstance(_config).AsSingle();
            Container.BindInterfacesTo<GameTimeService>().AsSingle();
        }
    }
}
