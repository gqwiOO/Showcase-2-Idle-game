using Core.Mechanics.Timer;
using Core.Mechanics.Timer.Service;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Installers.Core
{
    public class TimerInstaller: MonoInstaller
    {
        [SerializeField] private TimersProvider timersProvider;
        public override void InstallBindings()
        {
            Container.Bind<ITimerService>().To<TimerService>().AsSingle().WithArguments(timersProvider);
        }
    }
}