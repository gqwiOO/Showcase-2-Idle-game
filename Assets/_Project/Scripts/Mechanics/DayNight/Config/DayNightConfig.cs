using Mechanics.Config;
using UnityEngine;

namespace Mechanics.DayNight
{
    [CreateAssetMenu(menuName = "GameAssets/DayNight/DayNightConfig", fileName = "DayNightConfig", order = 0)]
    public class DayNightConfig : ScriptableObject
    {
        [Header("Time")]
        [Tooltip("Real seconds per 1 game hour")]
        [SerializeField] private float _secondsPerGameHour = 30f;

        [Header("Day")]
        [Tooltip("Day start hour (0-23)")]
        [SerializeField] private IntProperty _dayStartHour = IntProperty.Constant(7);

        [Tooltip("Day end hour (exclusive, 22 means day ends at 22:00)")]
        [SerializeField] private IntProperty _dayEndHour = IntProperty.Constant(22);

        [Header("Night")]
        [Tooltip("Night start hour (22)")]
        [SerializeField] private IntProperty _nightStartHour = IntProperty.Constant(22);

        [Tooltip("Night end hour (7, next day)")]
        [SerializeField] private IntProperty _nightEndHour = IntProperty.Constant(7);

        public float SecondsPerGameHour => _secondsPerGameHour;
        public int DayStartHour => _dayStartHour.Value;
        public int DayEndHour => _dayEndHour.Value;
        public int NightStartHour => _nightStartHour.Value;
        public int NightEndHour => _nightEndHour.Value;

        public int DayDurationHours => _dayEndHour.Value - _dayStartHour.Value;
        public int NightDurationHours => _nightEndHour.Value > _nightStartHour.Value
            ? 24 - _nightStartHour.Value + _nightEndHour.Value
            : _nightEndHour.Value - _nightStartHour.Value;
    }
}
