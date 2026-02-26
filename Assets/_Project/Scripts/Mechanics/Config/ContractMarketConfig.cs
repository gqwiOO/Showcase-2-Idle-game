using System;
using Mechanics.Product;
using UnityEngine;

namespace Mechanics.Config
{
    [Serializable]
    public struct TierRewardData
    {
        public float Reward;
        public float Reputation;
        public float Heat;
    }

    [CreateAssetMenu(menuName = "GameAssets/Config/ContractMarketConfig", fileName = "ContractMarketConfig", order = 0)]
    public class ContractMarketConfig : ScriptableObject
    {
        [Header("Offers")]
        [SerializeField] private int _offersPerNight = 6;

        [Header("Tier unlock (reputation required)")]
        [SerializeField] private float _tier2ReputationRequired = 50f;
        [SerializeField] private float _tier3ReputationRequired = 120f;

        [Header("Penalties")]
        [SerializeField] private float _penaltyHeatBase = 5f;
        [SerializeField] [Range(0f, 1f)] private float _injuryChanceBase = 0.25f;

        [Header("Tier weights (chance when unlocked)")]
        [SerializeField] [Range(0f, 1f)] private float _tier3Weight = 0.2f;
        [SerializeField] [Range(0f, 1f)] private float _tier2Weight = 0.5f;

        [Header("Required roles per offer")]
        [SerializeField] private int _requiredRolesMin = 1;
        [SerializeField] private int _requiredRolesMax = 4;

        [Header("Rewards by tier")]
        [SerializeField] private TierRewardData _tier1Rewards = new TierRewardData { Reward = 50f, Reputation = 5f, Heat = 3f };
        [SerializeField] private TierRewardData _tier2Rewards = new TierRewardData { Reward = 120f, Reputation = 12f, Heat = 6f };
        [SerializeField] private TierRewardData _tier3Rewards = new TierRewardData { Reward = 250f, Reputation = 25f, Heat = 10f };

        public int OffersPerNight => _offersPerNight;
        public float Tier2ReputationRequired => _tier2ReputationRequired;
        public float Tier3ReputationRequired => _tier3ReputationRequired;
        public float PenaltyHeatBase => _penaltyHeatBase;
        public float InjuryChanceBase => _injuryChanceBase;
        public float Tier3Weight => _tier3Weight;
        public float Tier2Weight => _tier2Weight;
        public int RequiredRolesMin => _requiredRolesMin;
        public int RequiredRolesMax => _requiredRolesMax;

        public TierRewardData GetRewardsByTier(ContractTier tier)
        {
            return tier switch
            {
                ContractTier.Tier1 => _tier1Rewards,
                ContractTier.Tier2 => _tier2Rewards,
                ContractTier.Tier3 => _tier3Rewards,
                _ => _tier1Rewards
            };
        }
    }
}
