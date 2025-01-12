using Zenject;

namespace Core.Mechanics.Timer.Service
{
    public class TimerService : ITimerService
    {
        private TimersProvider _timersProvider;
        
        [Inject]
        public TimerService(TimersProvider timersProvider)
        {
            _timersProvider = timersProvider;
        }
        
        public Timer CreateTimer(TimerData timerData)
        {
            Timer timer = new Timer(timerData);
            _timersProvider.AddTimer(timer);
            return timer;
        }
    }
}