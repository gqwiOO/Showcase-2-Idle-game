using System;
using System.Collections.Generic;
using Mechanics.Characters;
using Mechanics.Companies;

namespace Mechanics.GameSave
{
    
    [Serializable]
    public class GameData
    {
        public int Key;
        public List<CompanyData> Companies;
        public List<CharacterData> Characters;

        public ICharacterData MyCharacter;

        public float MoneyAmount;
    }
}