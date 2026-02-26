namespace Mechanics.DayNight
{
    /// <summary>
    /// Allows pushing player balance updates into the day/night service without creating a circular dependency.
    /// Implemented by GameTimeService; used by DayNightEconomyBridge.
    /// </summary>
    public interface IGameTimeBalanceInput
    {
        void NotifyBalanceChanged(float newBalance);
        void NotifyDayStarted(float startBalance);
    }
}
