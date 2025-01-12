using TMPro;
using UnityEngine;

namespace Core.Mechanics.Timer.View
{
    public abstract class BaseTimerView: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textField;

        public void Init(Timer timer)
        {
            timer.OnTick += Timer_OnTick;
            timer.OnTimerEnded += Timer_OnTimerEnded;
        }

        private void Timer_OnTimerEnded(object sender, TimerData e)
        {
            Timer timer = (Timer)sender;
            timer.OnTick -= Timer_OnTick;
            timer.OnTimerEnded -= Timer_OnTimerEnded;
        }

        protected abstract string ConvertTimerDataToString(TimerData data);

        private void Timer_OnTick(object sender, ITimer data)
        {
            string result = ConvertTimerDataToString(new TimerData(data));
            
            textField.text = result;
        }
    }
}