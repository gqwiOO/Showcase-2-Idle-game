using System;
using System.Collections.Generic;
using Mechanics.Characters;
using Mechanics.Companies;

namespace Mechanics.GameSession
{
    [Serializable]
    public class GameSession
    {
        public ICharacterData MyCharacterData;
        public List<ICharacterData> FreeEmployees;

        public List<ICompanyData> Companies;
    }
}