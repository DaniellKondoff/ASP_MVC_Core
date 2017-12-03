using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static ISet<T> ToHashSet<T>(this IEnumerable<T> enumerable)
        {
            return new HashSet<T>(enumerable);
        }
    }
}
