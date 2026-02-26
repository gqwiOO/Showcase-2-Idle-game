using UnityEngine;

namespace Mechanics.Config
{
    [CreateAssetMenu(menuName = "GameAssets/Config/ContractProductLimitsConfig", fileName = "ContractProductLimitsConfig", order = 0)]
    public class ContractProductLimitsConfig : ScriptableObject
    {
        [Header("Limits")]
        [SerializeField] private int _maxDevelopersOnProject = 3;
        [SerializeField] private int _maxAssigneesPerContract = 3;

        [Header("Default rewards (create product / company products)")]
        [SerializeField] private float _defaultRewardAmount = 50f;
        [SerializeField] private float _defaultReputationReward = 5f;
        [SerializeField] private float _defaultHeatReward = 3f;

        public int MaxDevelopersOnProject => _maxDevelopersOnProject;
        public int MaxAssigneesPerContract => _maxAssigneesPerContract;
        public float DefaultRewardAmount => _defaultRewardAmount;
        public float DefaultReputationReward => _defaultReputationReward;
        public float DefaultHeatReward => _defaultHeatReward;
    }
}
