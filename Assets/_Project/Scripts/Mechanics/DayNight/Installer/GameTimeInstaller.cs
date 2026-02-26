using UnityEngine;
using Zenject;

namespace Mechanics.DayNight
{
    public class GameTimeInstaller : MonoInstaller
    {
        [SerializeField] private DayNightConfig _config;

        public override void InstallBindings()
        {
            Container.Bind<DayNightConfig>().FromInstance(_config).AsSingle();
            Container.BindInterfacesAndSelfTo<GameTimeService>().AsSingle();
        }
    }
}
