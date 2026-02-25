using System;
using System.Collections.Generic;
using Mechanics.Characters;
using Mechanics.Developing.Data;
using Newtonsoft.Json;

namespace Mechanics.Product
{
    /// <summary>Unified contract entity: market offers and contracts in execution.</summary>
    [Serializable]
    public class ContractData : IContractData
    {
        public event Action OnIncomeChanged;
        public event Action<IContractData> OnCompleted;

        public string Key { get; set; }
        public string Name { get; set; }
        public ContractType Type { get; set; }
        public ContractTier Tier { get; set; }

        [JsonProperty("_requiredRoles")]
        public List<RoleType> RequiredRoles { get; set; } = new();

        IReadOnlyList<RoleType> IContractData.RequiredRoles => RequiredRoles;

        [JsonProperty("IncomePerMonth")]
        public float RewardAmount { get; set; }

        public float ReputationReward { get; set; } = 5f;
        public float HeatReward { get; set; } = 3f;
        public float PenaltyHeat { get; set; }
        public float InjuryChance { get; set; }

        public ContractState ContractState { get; set; }

        [JsonIgnore]
        IDevelopingData IContractData.ExecutionData => _executionData;

        [JsonProperty("_developingData")]
        public DevelopingData _executionData { get; set; }

        [JsonConstructor]
        public ContractData(string key, string name, float rewardAmount,
            [JsonProperty("ProductState")] ContractState contractState = ContractState.Executing)
        {
            Key = key;
            Name = name;
            RewardAmount = rewardAmount;
            ContractState = contractState;
        }

        /// <summary>Create market offer (Available state).</summary>
        public static ContractData CreateOffer(string key, string name, ContractType type, ContractTier tier,
            List<RoleType> requiredRoles, float rewardAmount, float reputationReward, float heatReward,
            float penaltyHeat, float injuryChance)
        {
            return new ContractData(key, name, rewardAmount, ContractState.Available)
            {
                Type = type,
                Tier = tier,
                RequiredRoles = requiredRoles ?? new List<RoleType>(),
                ReputationReward = reputationReward,
                HeatReward = heatReward,
                PenaltyHeat = penaltyHeat,
                InjuryChance = injuryChance
            };
        }

        /// <summary>Create contract in execution from market offer.</summary>
        public static ContractData CreateFromOffer(ContractData offer, DevelopingData executionData)
        {
            var c = new ContractData(offer.Key, offer.Name, offer.RewardAmount, ContractState.Executing)
            {
                Type = offer.Type,
                Tier = offer.Tier,
                RequiredRoles = new List<RoleType>(offer.RequiredRoles),
                ReputationReward = offer.ReputationReward,
                HeatReward = offer.HeatReward,
                PenaltyHeat = offer.PenaltyHeat,
                InjuryChance = offer.InjuryChance,
                _executionData = executionData
            };
            return c;
        }

        /// <summary>Create contract for legacy/create screen flow.</summary>
        public static ContractData Create(string key, string name, DevelopingData executionData,
            float rewardAmount = 50f, float reputationReward = 5f, float heatReward = 3f)
        {
            return new ContractData(key, name, rewardAmount, ContractState.Executing)
            {
                ReputationReward = reputationReward,
                HeatReward = heatReward,
                _executionData = executionData
            };
        }

        public void CompleteContract()
        {
            ContractState = ContractState.Completed;
            OnCompleted?.Invoke(this);
        }
    }
}
