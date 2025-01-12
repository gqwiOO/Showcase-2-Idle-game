using System.Collections.Generic;

namespace Mechanics.Characters
{
    public interface ICharactersProvider
    {
        ICharacterData GetCharacterByKey(string key);
        void AddCharacter(ICharacterData characterData);
        void AddMyCharacter(ICharacterData characterData);
        ICharacterData GetMyCharacter();
        List<ICharacterData> GetAllCharacter();
        
    }
}