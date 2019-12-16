using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IReadOnlyCollection<TSource> ToReadonlyArray<TSource>(this IEnumerable<TSource> source) => Array.AsReadOnly(source.ToArray());
    }
}
