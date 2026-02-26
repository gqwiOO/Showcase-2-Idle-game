using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Mechanics.Characters
{
    [Serializable]
    public class CharacterData : ICharacterData
    {
        public CharacterData(string name, int age, RoleType blackRole, PeacefulRoleType peacefulRole, int salary,
            string companyId, CharacterSkill characterSkill)
        {
            Age = age;
            BlackRole = blackRole;
            PeacefulRole = peacefulRole;
            Salary = salary;
            CompanyKey = companyId;
            CharacterSkill = characterSkill;
            Name = name;
            Key = GetHashCode().ToString();
        }

        [field: SerializeField]
        public string Key { get; set; }

        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        public int Age { get; set; }

        [field: SerializeField]
        public RoleType BlackRole { get; set; }

        [field: SerializeField]
        public PeacefulRoleType PeacefulRole { get; set; }

        [field: SerializeField]
        public CharacterSkill CharacterSkill { get; set; }

        [field: SerializeField]
        public int Salary { get; set; }

        [field: SerializeField]
        public string CompanyKey { get; set; }

        private List<string> _contracts = new();
        private List<string> _contractsInExecution = new();
        private bool _isInjured;

        public bool IsInjured => _isInjured;

        public IEnumerable<string> Contracts => _contracts;
        public IEnumerable<string> ContractsInExecution => _contractsInExecution;
        public int ContractsInExecutionCount => _contractsInExecution.Count;

        public void AddContract(string contractKey) => _contracts.Add(contractKey);
        public void DeleteContract(string contractKey) => _contracts.Remove(contractKey);
        public void AddContractInExecution(string contractKey) => _contractsInExecution.Add(contractKey);
        public void RemoveContractInExecution(string contractKey) => _contractsInExecution.Remove(contractKey);
        public void SetInjured(bool injured) => _isInjured = injured;

        public bool HasCompany() => !CompanyKey.IsEmpty();
        public void SetCompanyKey(string key) => CompanyKey = key;
    }
}