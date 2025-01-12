using System;

namespace Core.Mechanics.Timer
{
    [Serializable]
    public class TimerData
    {
        public float StartTime;
        public float EndTime;
        public float CurrentTime;
        
        public bool IgnoreTimeScale;

        public TimerData(float startTime, float endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            CurrentTime = StartTime;
        }

        public TimerData(ITimer timer)
        {
            StartTime = timer.StartTime;
            EndTime = timer.EndTime;
            CurrentTime = timer.StartTime;
        }
    }
}