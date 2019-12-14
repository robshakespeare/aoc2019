using System.Linq;
using Common;

namespace AoC.Day4
{
    public class Day4Solver : Solver<string>
    {
        public Day4Solver() : base(new InputLoaderDelegated<string>(() => "138241-674034"))
        {
        }

        public override long? SolvePart2(string input) => HowManyDifferentPasswordsWithinRange(input);

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
