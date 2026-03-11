using System;
using Mechanics.Companies;
using Mechanics.GameSave;

namespace Mechanics.Product
{
    public interface IContractService : IGameSaveDependent
    {
        event Action<IContractData> OnContractTaken;
        event Action<ContractCompletionResult> OnContractCompleted;
        event Action<IContractData> OnContractFailed;

        void TakeContract(ContractData contract, string ownerKey);
        void TakeContract(ContractData contract, ICompanyData owner);
        string GetRandomContractNameWithGenre(GameGenre genre, string currentName = "");
    }

    public struct ContractCompletionResult
    {
        public float DirtyMoneyReceived;
        public float ReputationAdded;
        public float HeatAdded;
    }
}
