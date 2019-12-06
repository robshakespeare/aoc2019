using System.Collections.Generic;
using System.IO;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Parses and returns each line in the input string.
        /// </summary>
        public static IEnumerable<string> ReadAllLines(this string s)
        {
            if (s == null)
            {
                yield break;
            }

            using var sr = new StringReader(s);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}
