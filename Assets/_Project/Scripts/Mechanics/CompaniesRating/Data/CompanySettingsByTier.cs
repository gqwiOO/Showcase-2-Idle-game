using System;
using UnityEngine;

namespace Mechanics.CompaniesRating.Data
{
    [Serializable]
    public class CompanySettingsByTier
    {
        public CompanyTier CompanyTier;
        public Vector2 EmployeesCount;
        public Vector2 EmployeeSalary;
        public Vector2 ProductsCountRange;
        public Vector2 ProductIncomeRange;
        
    }
}