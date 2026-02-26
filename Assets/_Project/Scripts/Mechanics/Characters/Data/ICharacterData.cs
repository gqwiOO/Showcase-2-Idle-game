using System.Collections.Generic;

namespace Mechanics.Characters
{
    public interface ICharacterData
    {
        public string Key { get; }
        public string Name { get; }
        public int Age { get; }
        /// <summary>Black (night) vacancy - crew role for contracts.</summary>
        public RoleType BlackRole { get; }
        /// <summary>Peaceful (day) vacancy - hotel job.</summary>
        public PeacefulRoleType PeacefulRole { get; }
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

    /// <summary>Black (night) role - crew specialty for contracts.</summary>
    public enum RoleType
    {
        None = 0,
        Assassin = 1,
        Handler = 2,
        Analyst = 3,
        Driver = 4,
        Medic = 5,
        Cleaner = 6,
        Gunsmith = 7,
        Technician = 8,
    }

    /// <summary>Peaceful (day) role - hotel job.</summary>
    public enum PeacefulRoleType
    {
        None = 0,
        Receptionist = 1,
        Housekeeper = 2,
        Concierge = 3,
        Cook = 4,
        Security = 5,
        Maintenance = 6,
        Waiter = 7,
        Manager = 8,
    }
}