using System;

namespace Core.Mechanics.Timer.View
{
    public class TimerViewMinutesSeconds : BaseTimerView
    {
        protected override string ConvertTimerDataToString(TimerData data)
        {
            TimeSpan time = TimeSpan.FromSeconds(data.CurrentTime);
            string result = time.ToString(@"mm\:ss");
            return result;
        }
    }
}