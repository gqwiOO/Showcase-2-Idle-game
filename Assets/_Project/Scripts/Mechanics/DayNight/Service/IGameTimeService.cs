using System;

namespace Mechanics.DayNight
{
    public interface IGameTimeService
    {
        DayNightPhase CurrentPhase { get; }
        int CurrentGameHour { get; }
        float ProgressInPhase { get; }
        float ProgressInDayCycle { get; }
        int DayNumber { get; }
        float TimeSpeed { get; set; }

        event Action<DayNightPhase> OnPhaseChanged;
        event Action<int> OnHourChanged;
        event Action<DaySummaryData> OnDaySummaryReady;

        void Init();

        void SetGameHour(int hour);
        void SetDay();
        void SetNight();
    }
}
