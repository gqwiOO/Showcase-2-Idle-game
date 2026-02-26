using UnityEngine;

namespace Mechanics.Config
{
    [CreateAssetMenu(menuName = "GameAssets/Config/GameEconomyConfig", fileName = "GameEconomyConfig", order = 0)]
    public class GameEconomyConfig : ScriptableObject
    {
        [Header("Income")]
        [Tooltip("Number of ticks per in-game month (used to convert monthly income to per-tick value)")]
        [SerializeField] private int _ticksPerMonth = 120;

        public int TicksPerMonth => _ticksPerMonth;
    }
}
