using System;
using System.Collections.Generic;

namespace Core.Extentions
{
    public static class ListExtensions
    {
        private static readonly Random RandomGenerator = new Random();

        public static T PickRandom<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                throw new InvalidOperationException("Cannot pick a random item from an empty or null list.");
            }

            int index = RandomGenerator.Next(list.Count);
            return list[index];
        }
        
        
    }

    public static class EnumExtensions
    {
        
        public static T GetRandomValueExceptFirst<T>(this Random random) where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            int randomIndex = random.Next(1, values.Length);
            return (T)values.GetValue(randomIndex);
        }
        
    }
}