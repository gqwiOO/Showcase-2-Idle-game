using System;
using System.Collections.Generic;
using System.Linq;
using Mechanics.Characters;
using Mechanics.DataSettings;
using UnityEngine;

namespace Mechanics.Hiring.Data
{
    [Serializable]
    public class CharactersGeneratingSettings: ISettingsData
    {
        public List<CharacterSettingsByLevel> Salaries;
        public List<string> Names;

        public int GetAge(CharacterSkill skill)
        {
            var ageRange = Salaries.First(item => item.CharacterSkill == skill).AgeRange;
            return new System.Random().Next((int)ageRange.x, (int)ageRange.y);
        }

        public int GetSalary(CharacterSkill skill)
        {
            var ageRange = Salaries.First(item => item.CharacterSkill == skill).SalaryRange;
            var salary = ConvertToTwoSignificantDigits(new System.Random().Next((int)ageRange.x, (int)ageRange.y));
            return salary;
        }
        private int ConvertToTwoSignificantDigits(int number)
        {
            int magnitude = (int)Math.Pow(10, (int)Math.Log10(number) - 1);
            return (number / magnitude) * magnitude;
        }
    }

    [Serializable]
    public class CharacterSettingsByLevel
    {
        public CharacterSkill CharacterSkill;
        public Vector2 SalaryRange;
        public Vector2 AgeRange;
    }
}