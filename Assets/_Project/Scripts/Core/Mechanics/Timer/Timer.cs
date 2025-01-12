using System;

namespace Core.Mechanics.Timer
{
    public class Timer: ITimer
    {
        private readonly TimerData _timerData;
        private readonly int _timerMultiplier;
    
        public bool IsActive { get; private set; }

        public float StartTime => _timerData.StartTime;
        public float EndTime => _timerData.EndTime;
        public float CurrentTime => _timerData.CurrentTime;

        public event EventHandler<ITimer> OnTick; 


        public event EventHandler<TimerData> OnTimerEnded; 
        public Timer(TimerData timerData)
        {
            _timerData = timerData;
            _timerMultiplier = StartTime > EndTime ? -1 : 1;
        }

        public void Start() => IsActive = true;

        public void Stop() => IsActive = false;

        public void Tick(float tickTime)
        {
            _timerData.CurrentTime += tickTime * _timerMultiplier;
            OnTick?.Invoke(this,this);
            CheckComplete();
        }

        private void CheckComplete()
        {
            if (CurrentTime > EndTime && _timerMultiplier == 1)
                OnTimerEnded?.Invoke(this,_timerData);
            else if (CurrentTime < EndTime && _timerMultiplier == -1)
                OnTimerEnded?.Invoke(this,_timerData);
        }
    }
}