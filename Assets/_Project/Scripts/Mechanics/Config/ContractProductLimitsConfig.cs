using UnityEngine;

namespace Mechanics.Config
{
    [CreateAssetMenu(menuName = "GameAssets/Config/ContractProductLimitsConfig", fileName = "ContractProductLimitsConfig", order = 0)]
    public class ContractProductLimitsConfig : ScriptableObject
    {
        [Header("Limits")]
        [SerializeField] private IntProperty _maxDevelopersOnProject = IntProperty.Constant(3);
        [SerializeField] private IntProperty _maxAssigneesPerContract = IntProperty.Constant(3);

        [Header("Default rewards (create product / company products)")]
        [SerializeField] private float _defaultRewardAmount = 50f;
        [SerializeField] private float _defaultReputationReward = 5f;
        [SerializeField] private float _defaultHeatReward = 3f;

        public int MaxDevelopersOnProject => _maxDevelopersOnProject.Value;
        public int MaxAssigneesPerContract => _maxAssigneesPerContract.Value;
        public float DefaultRewardAmount => _defaultRewardAmount;
        public float DefaultReputationReward => _defaultReputationReward;
        public float DefaultHeatReward => _defaultHeatReward;
    }
}
