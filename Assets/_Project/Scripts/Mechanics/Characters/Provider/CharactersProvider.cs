using System;
using System.Collections.Generic;
using System.Linq;

namespace Mechanics.Characters
{
    public class CharactersProvider : ICharactersProvider
    {
        private readonly Dictionary<string, ICharacterData> _characters = new ();
        
        private ICharacterData _myCharacter;

        public event Action<ICharacterData> OnMyCharacterInited;

        public ICharacterData GetCharacterByKey(string key)
        {
            _characters.TryGetValue(key, out var result);
            return result;
        }

        public List<ICharacterData> GetAllCharacter() 
            => _characters.Values.ToList();


        public void AddCharacter(ICharacterData characterData) 
            => _characters.TryAdd(characterData.Key, characterData);
        
        public void AddMyCharacter(ICharacterData characterData)
        {
            _characters.TryAdd(characterData.Key, characterData);
            _myCharacter = characterData;
            OnMyCharacterInited?.Invoke(_myCharacter);
        }

        public ICharacterData GetMyCharacter() 
            => _myCharacter;
    }
}