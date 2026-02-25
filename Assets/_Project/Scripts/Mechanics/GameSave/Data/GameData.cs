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
        [Newtonsoft.Json.JsonProperty("Products")]
        public List<ContractData> Contracts;

        public CharacterData MyCharacter;

        public float MoneyAmount; // Legacy, maps to CleanMoneyAmount when loading
        public float CleanMoneyAmount;
        public float DirtyMoneyAmount;
        public float ReputationAmount;
        public float HeatAmount;
    }
}