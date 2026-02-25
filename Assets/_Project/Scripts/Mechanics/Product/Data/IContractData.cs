using System;
using System.Collections.Generic;
using Mechanics.Characters;
using Mechanics.Developing.Data;

namespace Mechanics.Product
{
    public interface IContractData : IIncomeObject
    {
        string Key { get; }
        string Name { get; }
        ContractType Type { get; }
        ContractTier Tier { get; }
        IReadOnlyList<RoleType> RequiredRoles { get; }
        float RewardAmount { get; }
        float ReputationReward { get; }
        float HeatReward { get; }
        ContractState ContractState { get; }
        IDevelopingData ExecutionData { get; }

        event Action<IContractData> OnCompleted;

        void CompleteContract();
    }
}
