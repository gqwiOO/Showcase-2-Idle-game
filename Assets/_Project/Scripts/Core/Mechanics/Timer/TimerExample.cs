using Core.Mechanics.Timer.Service;
using Core.Mechanics.Timer.View;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core.Mechanics.Timer
{
    public class TimerExample: MonoBehaviour
    {
        [FormerlySerializedAs("m_timerView")] [SerializeField] private BaseTimerView baseTimerView;
        
        private ITimerService m_timerService;

        [Inject]
        private void Construct(ITimerService timerService)
        {
            m_timerService = timerService;
        }

        private void Start()
        {
            var timer = m_timerService.CreateTimer(new TimerData(100f, 0f));
            timer.Start();
            baseTimerView.Init(timer);
        }
    }
}