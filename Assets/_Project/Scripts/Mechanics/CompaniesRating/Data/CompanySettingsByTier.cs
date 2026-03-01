using System;
using Mechanics.Config;
using UnityEngine;

namespace Mechanics.CompaniesRating.Data
{
    [Serializable]
    public class CompanySettingsByTier
    {
        public CompanyTier CompanyTier;
        public IntProperty EmployeesCount;
        public IntProperty EmployeeSalary;
        public IntProperty ProductsCountRange;
        public Vector2 ProductIncomeRange;
    }
}