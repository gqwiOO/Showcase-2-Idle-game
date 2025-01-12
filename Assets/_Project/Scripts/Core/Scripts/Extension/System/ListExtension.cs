using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts.Extension.System
{
    public static class ListExtension
    {
        private static global::System.Random Random = new ();
        public static T PickRandom<T>(this IList<T> list) => list[Random.Next(0, list.Count)];
    }
}