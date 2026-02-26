using System.Collections.Generic;
using Mechanics.Characters;
using Mechanics.Hiring.Data;
using UnityEngine;

namespace Mechanics.Config
{
    [CreateAssetMenu(menuName = "GameAssets/Config/CharactersGeneratingSettings", fileName = "CharactersGeneratingSettings", order = 0)]
    public class CharactersGeneratingSettingsSO : ScriptableObject
    {
        [SerializeField] private List<CharacterSettingsByLevel> _salaries = new();
        [SerializeField] private List<string> _names = new();

        public CharactersGeneratingSettings GetSettings()
        {
            return new CharactersGeneratingSettings
            {
                Salaries = new List<CharacterSettingsByLevel>(_salaries),
                Names = new List<string>(_names)
            };
        }
    }
}
