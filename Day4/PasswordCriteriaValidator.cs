using System.Linq;

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
            // Note that `GroupBy` is not contiguous, but that's fine because the decreasing rule stops non-contiguous
            // matching numbers from ever come through to here.
            return passwordChars.GroupBy(c => c)
                .Any(grp => grp.Count() == 2); // Any set of 2 matching digits
        }
    }
}
