using System;
using System.Collections.Generic;
using System.Linq;

namespace comquiz
{
    static class ListUtils
    {
        private static readonly Random rng = new Random();

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;

        }

    }
}
