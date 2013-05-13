using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildIndicatron.Console
{
    public static class EnumerableHelper
    {
        private static Random _random = new Random();
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            var list = enumerable as IList<T> ?? enumerable.ToList();
            var count = list.Count();
            if (count <= 1)
                return list.FirstOrDefault();
            return list.Skip(_random.Next(0, count-1)).FirstOrDefault();
        }

        public static string StringJoin<T>(this IEnumerable<T> enumerable,string join)
        {
            return enumerable != null ? string.Join(@join, enumerable.Select(x => x.ToString()).ToArray()) : null;
        }
    }
}