using System;
using System.Collections.Generic;
using Mechanics.Product;
using Newtonsoft.Json;
using UnityEngine;

namespace Mechanics.Companies
{
    public interface ICompanyData : IIncomeObject
    {
        public string Key { get; }
        public string Name { get; }
        public IEnumerable<string> Employees { get; }
        public string Owner { get; }
        public IEnumerable<string> Contracts { get; }
        void AddContract(string contractKey);
        void DeleteContract(string contractKey);
        void AddEmployee(string employee);
        void DeleteEmployee(string employee);
    }

    [Serializable]
    public class CompanyData : ICompanyData
    {
        [JsonConstructor]
        public CompanyData(string key, string name, string owner)
        {
            Name = name;
            Key = key;
            Owner = owner;
        }
        public CompanyData(string name, string owner)
        {
            Name = name;
            Key = owner.GetHashCode().ToString();
            Owner = owner;
        }

        [field: SerializeField]
        public string Key { get; private set; }
        
        [field: SerializeField]
        public string Name { get; private set; }
        
        [field: SerializeField]
        public string Owner { get; private set; }
        
        private List<string> _employees { get; set; } = new();

        [field: SerializeField]
        public IEnumerable<string> Employees => _employees;

        [JsonProperty("_products")]
        private List<string> _contracts { get; set; } = new();

        public IEnumerable<string> Contracts => _contracts;

        public event Action OnIncomeChanged;
        public event Action OnContractsChanged;
        public event Action OnEmployeesChanged;

        public void AddContract(string contractKey)
        {
            _contracts.Add(contractKey);
            OnContractsChanged?.Invoke();
        }

        public void DeleteContract(string contractKey)
        {
            _contracts.Remove(contractKey);
            OnContractsChanged?.Invoke();
        }

        public void AddEmployee(string employee)
        {
            _employees.Add(employee);
            OnEmployeesChanged?.Invoke();
        }
        
        public void DeleteEmployee(string employee)
        {
            _employees.Remove(employee);
            OnEmployeesChanged?.Invoke();
        }
    }
}