using System;
using System.Collections.Generic;
using Mechanics.Product;

namespace Mechanics.CompaniesRating.Generator
{
    [Serializable]
    public class ProductsSettingsByGenre
    {
        public GameGenre Genre;
        public List<string> Names;
    }
}