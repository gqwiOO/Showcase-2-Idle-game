using Mechanics.Characters;
using UnityEngine;

namespace Mechanics.Config
{
    [CreateAssetMenu(menuName = "GameAssets/Config/ContractExecutionConfig", fileName = "ContractExecutionConfig", order = 0)]
    public class ContractExecutionConfig : ScriptableObject
    {
        [Header("Duration")]
        [SerializeField] private IntProperty _baseExecutionTimeSeconds = IntProperty.Constant(60);

        [Header("Time reduction % by skill")]
        [SerializeField] private float _beginnerTimeBoostPercent = 5f;
        [SerializeField] private float _intermediateTimeBoostPercent = 8f;
        [SerializeField] private float _advancedTimeBoostPercent = 15f;
        [SerializeField] private float _expertTimeBoostPercent = 20f;

        public int BaseExecutionTimeSeconds => _baseExecutionTimeSeconds.Value;

        public float GetTimeBoostPercent(CharacterSkill skill)
        {
            switch (skill)
            {
                case CharacterSkill.Beginner: return _beginnerTimeBoostPercent;
                case CharacterSkill.Intermediate: return _intermediateTimeBoostPercent;
                case CharacterSkill.Advanced: return _advancedTimeBoostPercent;
                case CharacterSkill.Expert: return _expertTimeBoostPercent;
                default: return 0f;
            }
        }
    }
}
