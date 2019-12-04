using System.Collections.Generic;

namespace Day4
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

        private static bool Validate(IEnumerable<char> passwordChars)
        {
            char? previousChar = null;
            var containsDoubleChars = false;

            foreach (var currentChar in passwordChars)
            {
                if (previousChar != null)
                {
                    // Must contain two adjacent digits that are the same (like 22 in 122345)
                    if (previousChar == currentChar)
                    {
                        containsDoubleChars = true;
                    }

                    // Going from left to right, the digits never decrease;
                    // they only ever increase or stay the same (like 111123 or 135679).
                    if (currentChar < previousChar)
                    {
                        return false;
                    }
                }

                previousChar = currentChar;
            }

            return containsDoubleChars;
        }
    }
}
