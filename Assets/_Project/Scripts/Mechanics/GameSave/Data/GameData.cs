using System;
using System.Collections.Generic;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.Product;

namespace Mechanics.GameSave
{
    [Serializable]
    public class GameData
    {
        public int Key;
        public List<CompanyData> Companies;
        public List<CharacterData> Characters;
        public List<GameProductData> Products;

        public CharacterData MyCharacter;

        public float MoneyAmount;
    }
}