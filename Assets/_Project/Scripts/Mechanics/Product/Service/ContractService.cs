using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Extension.System;
using Cysharp.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.GameSave;
using Mechanics.Income;
using Mechanics.Product.JSON;
using Mechanics.Product.Provider;
using UnityEngine;
using Zenject;

namespace Mechanics.Product
{
    public class ContractService : IContractService, IInitializable
    {
        private ProductNamesJsonStorage _jsonNamesStorage;
        private ICharactersProvider _charactersProvider;
        private IContractsProvider _contractsProvider;
        private IGameEconomyService _gameEconomyService;

        private List<IContractData> _contractsInExecution = new();

        private const int BaseExecutionTimeSeconds = 60;

        public event Action<IContractData> OnContractTaken;
        public event Action<ContractCompletionResult> OnContractCompleted;

        [Inject]
        private void Construct(ICharactersProvider charactersProvider, IContractsProvider contractsProvider,
            IGameEconomyService gameEconomyService)
        {
            _gameEconomyService = gameEconomyService;
            _contractsProvider = contractsProvider;
            _charactersProvider = charactersProvider;
        }

        public void Initialize()
        {
            _jsonNamesStorage = new ProductNamesJsonStorage();
            _jsonNamesStorage.Load();
        }

        public void InjectGameSave(GameData gameSave)
        {
            if (gameSave.Contracts == null) return;
            foreach (var contract in gameSave.Contracts)
            {
                _contractsProvider.AddContract(contract);
                if (contract.ContractState == ContractState.Executing)
                    StartExecutingContract(contract);
            }
        }

        public void TakeContract(ContractData contract, string ownerKey)
        {
            var character = _charactersProvider.GetCharacterByKey(ownerKey);
            character.AddContract(contract.Key);

            _contractsProvider.AddContract(contract);
            OnContractTaken?.Invoke(contract);

            AddContractToAssignees(contract);
            StartExecutingContract(contract);
        }

        public void TakeContract(ContractData contract, ICompanyData owner)
        {
            // TODO: company contracts
        }

        private void AddContractToAssignees(IContractData contractData)
        {
            if (contractData.ExecutionData?.Developers == null) return;
            foreach (var assignee in contractData.ExecutionData.Developers)
            {
                var character = _charactersProvider.GetCharacterByKey(assignee.Key);
                character?.AddContractInExecution(contractData.Key);
            }
        }

        private void RemoveContractFromAssignees(IContractData contractData)
        {
            if (contractData.ExecutionData?.Developers == null) return;
            foreach (var assignee in contractData.ExecutionData.Developers)
            {
                var character = _charactersProvider.GetCharacterByKey(assignee.Key);
                character?.RemoveContractInExecution(contractData.Key);
            }
        }

        private void StartExecutingContract(IContractData contractData)
        {
            RunExecutionTask(contractData).Forget();
        }

        private async UniTask RunExecutionTask(IContractData contractData)
        {
            _contractsInExecution.Add(contractData);

            var executionSpeed = GetExecutionSpeedInSeconds(contractData);
            while (contractData.ExecutionData.CurrentProgress < 1f)
            {
                contractData.ExecutionData.AddProgress(Time.deltaTime / executionSpeed);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            RemoveContractFromAssignees(contractData);
            contractData.CompleteContract();

            var reward = contractData.RewardAmount;
            var reputation = contractData.ReputationReward;
            var heat = contractData.HeatReward;

            _gameEconomyService.AddDirtyMoney(reward);
            _gameEconomyService.AddReputation(reputation);
            _gameEconomyService.AddHeat(heat);

            OnContractCompleted?.Invoke(new ContractCompletionResult
            {
                DirtyMoneyReceived = reward,
                ReputationAdded = reputation,
                HeatAdded = heat
            });

            _contractsInExecution.Remove(contractData);
        }

        private float GetExecutionSpeedInSeconds(IContractData contractData)
        {
            float result = BaseExecutionTimeSeconds;
            foreach (var assignee in contractData.ExecutionData.Developers)
                result -= result / 100 * GetAssigneeTimeBoost(assignee);
            return result;
        }

        public string GetRandomContractNameWithGenre(GameGenre genre, string currentName = "")
        {
            return _jsonNamesStorage
                .Get()
                .GenreNamesList
                .Where(list => list.Genre == genre)
                .ToList().PickRandom()
                .Name;
        }

        private float GetAssigneeTimeBoost(ICharacterData characterData)
        {
            switch (characterData.CharacterSkill)
            {
                case CharacterSkill.Beginner: return 5f;
                case CharacterSkill.Intermediate: return 8f;
                case CharacterSkill.Advanced: return 15f;
                case CharacterSkill.Expert: return 20f;
                default: return 0f;
            }
        }
    }
}
