using System.Collections.Generic;
using System.Linq;
using Mechanics.Characters;
using Sirenix.Utilities;
using Zenject;

namespace Mechanics.Product.Provider
{
    public interface IContractsProvider
    {
        IContractData GetContract(string key);
        IEnumerable<IContractData> GetAllContractsOfCharacter(string characterKey);
        IEnumerable<IContractData> GetAllContracts();
        void AddContract(IContractData contract);
        void RemoveContract(string key);
    }

    public class ContractsProvider : IContractsProvider
    {
        private readonly Dictionary<string, IContractData> _contracts = new();
        private ICharactersProvider _charactersProvider;

        [Inject]
        private void Construct(ICharactersProvider charactersProvider)
        {
            _charactersProvider = charactersProvider;
        }

        public IContractData GetContract(string key)
        {
            _contracts.TryGetValue(key, out var contract);
            return contract;
        }

        public IEnumerable<IContractData> GetAllContractsOfCharacter(string characterKey)
        {
            var character = _charactersProvider.GetCharacterByKey(characterKey);
            var contractKeys = character.Contracts;

            List<IContractData> result = new(contractKeys.Count());
            contractKeys.ForEach(k =>
            {
                _contracts.TryGetValue(k, out var contract);
                if (contract != null) result.Add(contract);
            });
            return result;
        }

        public IEnumerable<IContractData> GetAllContracts() => _contracts.Values;

        public void AddContract(IContractData contract)
        {
            _contracts.TryAdd(contract.Key, contract);
        }

        public void RemoveContract(string key)
        {
            _contracts.Remove(key);
        }
    }
}