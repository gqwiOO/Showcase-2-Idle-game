using System;
using System.Collections.Generic;
using Mechanics.DataSettings;
using UnityEngine;

namespace Mechanics.CompaniesRating.Generator
{
    [Serializable]
    public class ProductsGeneratingSettings: ISettingsData
    {
        public Vector2Int WeakProductIncomeRange;
        public Vector2Int MidProductIncomeRange;
        public Vector2Int SuccessfulProductIncomeRange;
        
        public Vector3 ProductProgressChancesRange;
        
        public List<ProductsSettingsByGenre> ProductsSettingsByGenres;
    }
}