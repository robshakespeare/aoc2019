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

        public override long? SolvePart2(string input) => SolvePart2(input, 200).CountInfestedTiles();

        public GridceptionManager SolvePart2(string input, int numberOfRepetitions)
        {
            var gridceptionManager = GridceptionManager.Load(input);

            for (var i = 0; i < numberOfRepetitions; i++)
            {
                gridceptionManager.Update();
            }

            return gridceptionManager;
        }
    }
}
