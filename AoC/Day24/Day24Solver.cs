using System.Collections.Generic;
using Common;

namespace AoC.Day24
{
    public class Day24Solver : SolverReadAllText
    {
        public override long? SolvePart1(string input)
        {
            var grid = Grid.Load(input);

            var biodiversityRatings = new HashSet<long>();
            long? result = null;

            while (result == null)
            {
                grid.Update();

                var biodiversityRating = grid.CalculateBiodiversityRating();

                if (!biodiversityRatings.Add(biodiversityRating))
                {
                    result = biodiversityRating;
                }
            }

            return result.Value;
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
