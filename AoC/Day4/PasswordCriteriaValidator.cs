using System.Collections.Generic;
using System.Linq;

namespace AoC.Day4
{
    public class PasswordCriteriaValidator
    {
        public bool IsValid(int number)
        {
            // Must be a six-digit number.
            if (number < 111111 || number > 999999)
            {
                return false;
            }

            return Validate(number.ToString());
        }

        private static bool Validate(string passwordChars)
        {
            char? previousChar = null;

            foreach (var currentChar in passwordChars)
            {
                if (previousChar != null)
                {
                    // Going from left to right, the digits never decrease;
                    // they only ever increase or stay the same (like 111123 or 135679).
                    if (currentChar < previousChar)
                    {
                        return false;
                    }
                }

                previousChar = currentChar;
            }

            // Must contain at least one set of two adjacent matching digits that are not part of a larger group of matching digits
            return GetAdjacentGroupLengths(passwordChars).Any(groupLength => groupLength == 2);
        }

        public static IEnumerable<int> GetAdjacentGroupLengths(string passwordChars)
        {
            char? previousChar = null;
            var groupLength = 0;

            foreach (var currentChar in passwordChars)
            {
                if (currentChar != previousChar && previousChar != null)
                {
                    yield return groupLength;
                    groupLength = 1;
                }
                else
                {
                    groupLength++;
                }

                previousChar = currentChar;
            }

            if (groupLength > 0)
            {
                yield return groupLength;
            }
        }
    }
}
