using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildIndicatron.Core
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
            return list.Skip(_random.Next(0, count)).FirstOrDefault();
        }

        public static string StringJoin<T>(this IEnumerable<T> enumerable,string join = ", ")
        {
            return enumerable != null ? string.Join(@join, enumerable.Select(x => x.ToString()).ToArray()) : null;
        }

	    public static int LastCharInRange(this string text, char c, int maxLength)
	    {
		    return text.Substring(0, maxLength).LastIndexOf(c);
	    }
    }
}