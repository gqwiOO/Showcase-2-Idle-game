using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Extension.System;
using Cysharp.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.Config;
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
        private readonly ICharactersProvider _charactersProvider;
        private readonly IContractsProvider _contractsProvider;
        private readonly IGameEconomyService _gameEconomyService;
        private readonly ContractExecutionConfig _executionConfig;

        private List<IContractData> _contractsInExecution = new();

        public event Action<IContractData> OnContractTaken;
        public event Action<ContractCompletionResult> OnContractCompleted;
        public event Action<IContractData> OnContractFailed;

        [Inject]
        public ContractService(
            ICharactersProvider charactersProvider,
            IContractsProvider contractsProvider,
            IGameEconomyService gameEconomyService,
            ContractExecutionConfig executionConfig)
        {
            _charactersProvider = charactersProvider;
            _contractsProvider = contractsProvider;
            _gameEconomyService = gameEconomyService;
            _executionConfig = executionConfig;
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
            float result = _executionConfig.BaseExecutionTimeSeconds;
            foreach (var assignee in contractData.ExecutionData.Developers)
                result -= result / 100 * _executionConfig.GetTimeBoostPercent(assignee.CharacterSkill);
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

    }
}
