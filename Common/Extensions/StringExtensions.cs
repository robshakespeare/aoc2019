using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Parses and returns each line in the input string.
        /// </summary>
        public static string[] ReadAllLines(this string s)
        {
            IEnumerable<string> ReadAllLinesEnumerable()
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

            return ReadAllLinesEnumerable().ToArray();
        }
    }
}
