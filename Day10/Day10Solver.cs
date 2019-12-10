using System.Collections.Generic;
using System.Linq;
using Common;

namespace Day10
{
    public class Day10Solver : SolverReadAllLines
    {
        private static Vector[] ParseAsteroidLocations(IEnumerable<string> inputLines) =>
            inputLines
                .SelectMany((line, y) => line.Select((chr, x) => chr == '#' ? new Vector(x, y) : (Vector?)null))
                .Where(asteroid => asteroid != null)
                .Select(asteroid => asteroid.Value)
                .ToArray();

        public override long? SolvePart1(string[] inputLines)
        {
            // For each asteroid, ignore itself, and then group by their absolute normal vectors, then count the number of groups
            // That value is the number of asteroids it has line of sight of
            // Then get the maximum value of those values, that gives us how many other asteroids can be detected from the best location.

            var asteroids = ParseAsteroidLocations(inputLines);

            var bestLocation = asteroids
                .Select(thisAsteroid =>
                {
                    ////var test = asteroids.Where(otherAsteroid => !otherAsteroid.Equals(thisAsteroid))
                    ////    .Select(otherAsteroid => new {otherAsteroid, norm = GetAbsoluteNormalBetweenVectors(otherAsteroid, thisAsteroid)}.ToString())
                    ////    .ToArray();

                    var groups = asteroids.Where(otherAsteroid => !otherAsteroid.Equals(thisAsteroid))
                        .Select(otherAsteroid => new {otherAsteroid, norm = GetNormalBetweenVectors(thisAsteroid, otherAsteroid)})
                        .GroupBy(x => x.norm);

                    ////var groupsDbg = groups.Select(x => new
                    ////{
                    ////    x.Key,
                    ////    count = x.Count(),
                    ////    riods = string.Join(", ", x.Select(y => y.otherAsteroid))
                    ////}.ToString()).ToArray();

                    var countOfAsteroidsInLineOfSight = groups.Count();
                    return new {thisAsteroid, countOfAsteroidsInLineOfSight};
                })
                .OrderByDescending(x => x.countOfAsteroidsInLineOfSight)
                .First();

            return bestLocation.countOfAsteroidsInLineOfSight;
        }

        public override long? SolvePart2(string[] inputLines)
        {
            return base.SolvePart2(inputLines);
        }

        public Vector GetNormalBetweenVectors(Vector v1, Vector v2) => (v2 - v1).Normal;
    }
}
