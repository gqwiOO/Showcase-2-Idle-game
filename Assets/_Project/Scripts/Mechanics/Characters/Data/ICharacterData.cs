using System.Collections.Generic;

namespace Mechanics.Characters
{
    public interface ICharacterData
    {
        public string Key { get; }
        public string Name { get; }
        public int Age { get; }
        public RoleType Role { get; }
        public CharacterSkill CharacterSkill { get; }
        public int Salary { get; }
        public string CompanyKey { get; }
        public bool IsInjured { get; }

        /// <summary>Contract keys (was Products).</summary>
        public IEnumerable<string> Contracts { get; }
        /// <summary>Contracts in execution (was ProductsInDeveloping).</summary>
        public IEnumerable<string> ContractsInExecution { get; }
        public int ContractsInExecutionCount { get; }

        void AddContract(string contractKey);
        void DeleteContract(string contractKey);
        void AddContractInExecution(string contractKey);
        void RemoveContractInExecution(string contractKey);
        void SetInjured(bool injured);

        bool HasCompany();
        void SetCompanyKey(string key);
    }

    public enum RoleType
    {
        None = 0,
        Developer = 1,
        ArtDesigner = 2,
    }
}