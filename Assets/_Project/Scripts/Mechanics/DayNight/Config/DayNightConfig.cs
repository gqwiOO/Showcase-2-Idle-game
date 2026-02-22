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
        [SerializeField] private int _dayStartHour = 7;

        [Tooltip("Day end hour (exclusive, 22 means day ends at 22:00)")]
        [SerializeField] private int _dayEndHour = 22;

        [Header("Night")]
        [Tooltip("Night start hour (22)")]
        [SerializeField] private int _nightStartHour = 22;

        [Tooltip("Night end hour (7, next day)")]
        [SerializeField] private int _nightEndHour = 7;

        public float SecondsPerGameHour => _secondsPerGameHour;
        public int DayStartHour => _dayStartHour;
        public int DayEndHour => _dayEndHour;
        public int NightStartHour => _nightStartHour;
        public int NightEndHour => _nightEndHour;

        public int DayDurationHours => _dayEndHour - _dayStartHour;
        public int NightDurationHours => _nightEndHour > _nightStartHour
            ? 24 - _nightStartHour + _nightEndHour
            : _nightEndHour - _nightStartHour;
    }
}
