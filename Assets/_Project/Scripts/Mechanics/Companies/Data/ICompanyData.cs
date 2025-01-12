using System;
using System.Collections.Generic;
using Mechanics.Characters;
using Mechanics.Product;
using Newtonsoft.Json;
using UnityEngine;

namespace Mechanics.Companies
{
    public interface ICompanyData: IIncomeObject
    {
        public string Key { get; }
        public string Name { get; }
        public IEnumerable<string> Employees { get; }
        public string Owner { get; }
        public IEnumerable<string> Products { get; }
        void AddProduct(string productData);
        void DeleteProduct(string productData);
        void AddEmployee(string employee);
        void DeleteEmployee(string product);
    }

    [Serializable]
    public class CompanyData : ICompanyData
    {
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
        private List<string> _products { get; set; } = new();
        
        [field: SerializeField]
        public IEnumerable<string> Products => _products;

        public event Action OnIncomeChanged;
        
        public event Action OnProductsChanged;
        
        public event Action OnEmployeesChanged;
        
        public void AddProduct(string productData)
        {
            _products.Add(productData);
            OnProductsChanged?.Invoke();
        }
        public void DeleteProduct(string productData)
        {
            _products.Remove(productData);
            OnProductsChanged?.Invoke();
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