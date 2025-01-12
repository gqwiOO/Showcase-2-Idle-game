namespace Core.Mechanics.Timer.Service
{
    public interface ITimerService
    {
        Timer CreateTimer(TimerData timerData);
    }
}