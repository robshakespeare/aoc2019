using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IReadOnlyList<TSource> ToReadonlyArray<TSource>(this IEnumerable<TSource> source) => Array.AsReadOnly(source.ToArray());
    }
}
