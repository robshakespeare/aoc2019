using System.Linq;

namespace Day4
{
    public class Solver
    {
        public int HowManyDifferentPasswordsWithinRange(string range)
        {
            var (start, rangeLength) = ParseRangeString(range);
            var passwordCriteriaValidator = new PasswordCriteriaValidator();

            return Enumerable.Range(start, rangeLength)
                .Select(passwordCriteriaValidator.IsValid)
                .Count(isValid => isValid);
        }

        public (int start, int rangeLength) ParseRangeString(string range)
        {
            var rangeItems = range.Split('-');
            var start = int.Parse(rangeItems[0]);
            var end = int.Parse(rangeItems[1]);
            var rangeLength = end - start + 1;

            return (start, rangeLength);
        }
    }
}
