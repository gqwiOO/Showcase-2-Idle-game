using UnityEngine;

namespace Mechanics.DayNight
{
    public abstract class DayResultView : MonoBehaviour
    {
        public abstract bool SupportsData(IDayResultData data);
        public abstract void Init(IDayResultData data);
    }
}
