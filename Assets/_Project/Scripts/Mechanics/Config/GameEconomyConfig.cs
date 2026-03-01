using UnityEngine;

namespace Mechanics.Config
{
    [CreateAssetMenu(menuName = "GameAssets/Config/GameEconomyConfig", fileName = "GameEconomyConfig", order = 0)]
    public class GameEconomyConfig : ScriptableObject
    {
        [Header("New game")]
        [Tooltip("Clean money given to the player when starting a new save")]
        [SerializeField] private float _startMoney = 0f;

        [Header("Income")]
        [Tooltip("Number of ticks per in-game month (used to convert monthly income to per-tick value)")]
        [SerializeField] private IntProperty _ticksPerMonth = IntProperty.Constant(120);

        public float StartMoney => _startMoney;
        public int TicksPerMonth => _ticksPerMonth.Value;
    }
}
