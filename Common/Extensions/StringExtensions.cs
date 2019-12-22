using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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

        private static readonly Regex LineEndingsRegex = new Regex(@"\r\n|\n\r|\n|\r", RegexOptions.Compiled);

        /// <summary>
        /// Normalizes the line endings in the specified string, so that all the line endings match the current environment's line endings.
        /// </summary>
        public static string NormalizeLineEndings(this string s) => NormalizeLineEndings(s, Environment.NewLine);

        /// <summary>
        /// Normalizes the line endings in the specified string, so that all the line endings match the specified line endings.
        /// </summary>
        public static string NormalizeLineEndings(this string s, string newLine) => LineEndingsRegex.Replace(s, newLine);
    }
}
