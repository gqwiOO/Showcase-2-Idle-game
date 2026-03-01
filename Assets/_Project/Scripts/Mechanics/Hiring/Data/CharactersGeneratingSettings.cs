using System;
using System.Collections.Generic;
using System.Linq;
using Mechanics.Characters;
using Mechanics.Config;
using Mechanics.DataSettings;
using UnityEngine;

namespace Mechanics.Hiring.Data
{
    [Serializable]
    public class CharactersGeneratingSettings : ISettingsData
    {
        public List<CharacterSettingsByLevel> Salaries;
        public List<string> Names;

        public int GetAge(CharacterSkill skill)
        {
            return Salaries.First(item => item.CharacterSkill == skill).AgeRange.Value;
        }

        public int GetSalary(CharacterSkill skill)
        {
            var salary = Salaries.First(item => item.CharacterSkill == skill).SalaryRange.Value;
            return ConvertToTwoSignificantDigits(salary);
        }

        private static int ConvertToTwoSignificantDigits(int number)
        {
            if (number <= 0) return number;
            int magnitude = (int)Math.Pow(10, (int)Math.Log10(number) - 1);
            return (number / magnitude) * magnitude;
        }
    }

    [Serializable]
    public class CharacterSettingsByLevel
    {
        public CharacterSkill CharacterSkill;
        public IntProperty SalaryRange;
        public IntProperty AgeRange;
    }
}