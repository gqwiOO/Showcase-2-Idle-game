using System.Collections.Generic;
using Mechanics.Product;

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
        
        public IEnumerable<string> Products { get; }
        public IEnumerable<string> ProductsInDeveloping { get; }
        public int ProductsInDevelopingCount { get;  }

        // public int Efficiency { get; }
        void AddProduct(string productData);
        void DeleteProduct(string productData);

        public void AddDevelopmentProduct(string productData);

        public void RemoveDevelopmentProduct(string productData);
        
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