using System;
using System.Collections.Generic;
using Mechanics.Product;
using ModestTree;
using UnityEngine;

namespace Mechanics.Characters
{
    [Serializable]
    public class CharacterData : ICharacterData
    {

        public CharacterData(string name, int age, RoleType role, int salary, string companyId, CharacterSkill characterSkill)
        {
            Age = age;
            Role = role;
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
        public RoleType Role { get; set; }
        
        [field: SerializeField]
        public CharacterSkill CharacterSkill { get; set; }
        
        [field: SerializeField]
        public int Salary { get; set; }
        
        [field: SerializeField]
        public string CompanyKey { get; set; }
        private List<string> _products = new (); 
        private List<string> _productsInDeveloping = new ();
        public IEnumerable<string> Products => _products;
        public IEnumerable<string> ProductsInDeveloping => _productsInDeveloping;
        public int ProductsInDevelopingCount => _productsInDeveloping.Count;

        public void AddProduct(string productData) 
            => _products.Add(productData);

        public void AddDevelopmentProduct(string productData) 
            => _productsInDeveloping.Add(productData);
        
        public void RemoveDevelopmentProduct(string productData) 
            => _productsInDeveloping.Remove(productData);

        public void DeleteProduct(string productData)  => _products.Remove(productData);

        public bool HasCompany() => !CompanyKey.IsEmpty();
        
        public void SetCompanyKey(string key) 
            => CompanyKey =  key;
    }
}