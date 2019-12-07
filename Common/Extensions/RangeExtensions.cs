using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
    public static class RangeExtensions
    {
        public static int[] ToArray(this Range range)
        {
            return ToEnumerable(range).ToArray();
        }

        public static IEnumerable<int> ToEnumerable(Range range)
        {
            for (var i = range.Start.Value; i < range.End.Value; i++)
            {
                yield return i;
            }
        }
    }
}
