using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AoC.Day17
{
    public static class PatternFinder
    {
        public const int MaxDepthOfPatternSearch = 3;

        public static IReadOnlyList<string>? FindFirstMatchingPattern(string input, int depth = 0)
        {
            if (depth >= MaxDepthOfPatternSearch)
            {
                return null;
            }

            foreach (var token in EnumeratePossibleTokens(input))
            {
                var stripped = RemoveToken(token, input);

                // If we have nothing left after using this token, then we have found our match, so return!
                if (stripped.Length == 0)
                {
                    return new[] {token};
                }

                var result = FindFirstMatchingPattern(stripped, depth + 1);

                if (result != null)
                {
                    return result.Prepend(token).ToReadOnlyArray();
                }
            }

            return null;
        }

        private static IEnumerable<string> EnumeratePossibleTokens(string input, int maxTokenLength = 20)
        {
            var currentToken = "";

            foreach (var part in input.Split(','))
            {
                currentToken += currentToken == "" ? part : ',' + part;

                if (currentToken.Length <= maxTokenLength)
                {
                    yield return currentToken;
                }
                else
                {
                    yield break;
                }
            }
        }

        private static string RemoveToken(string token, string input) => input.Replace(token, "").Replace(",,", ",").Trim(',');
    }
}
