using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace Day10
{
    public class Day10Solver : SolverReadAllLines
    {
        public static void Main() => new Day10Solver().Run();

        public override long? SolvePart1(string[] inputLines)
        {
            // For each asteroid, ignore itself, and then group by their absolute normal vectors, then count the number of groups
            // That value is the number of asteroids it has line of sight of
            // Then get the maximum value of those values, that gives us how many other asteroids can be detected from the best location.

            var asteroids = ParseAsteroidLocations(inputLines);

            var bestLocation = GetBestLocation(asteroids);

            return bestLocation.countOfAsteroidsInLineOfSight;
        }

        private static Vector[] ParseAsteroidLocations(IEnumerable<string> inputLines) =>
            inputLines
                .SelectMany((line, y) => line.Select((chr, x) => chr == '#' ? new Vector(x, y) : (Vector?)null))
                .Where(asteroid => asteroid != null)
                .Select(asteroid => asteroid.Value)
                .ToArray();

        private (Vector asteroid, int countOfAsteroidsInLineOfSight) GetBestLocation(Vector[] asteroids)
        {
            var bestLocation = asteroids
                .Select(thisAsteroid =>
                {
                    var groups = asteroids.Where(otherAsteroid => !otherAsteroid.Equals(thisAsteroid))
                        .Select(otherAsteroid => new {otherAsteroid, norm = GetNormalBetweenVectors(thisAsteroid, otherAsteroid)})
                        .GroupBy(x => x.norm);
                    var countOfAsteroidsInLineOfSight = groups.Count();
                    return new {thisAsteroid, countOfAsteroidsInLineOfSight};
                })
                .OrderByDescending(x => x.countOfAsteroidsInLineOfSight)
                .First();
            return (bestLocation.thisAsteroid, bestLocation.countOfAsteroidsInLineOfSight);
        }

        public Vector GetNormalBetweenVectors(Vector v1, Vector v2) => (v2 - v1).Normal;

        public override long? SolvePart2(string[] inputLines)
        {
            // The laser starts by pointing up and always rotates clockwise, vaporizing any asteroid it hits.

            var asteroids = ParseAsteroidLocations(inputLines);

            var bestLocation = GetBestLocation(asteroids);

            var allOtherAsteroids = asteroids.Where(otherAsteroid => !otherAsteroid.Equals(bestLocation.asteroid)).ToArray();

            var target = FireLaserRoundUntilTargetReached(bestLocation.asteroid, allOtherAsteroids)
                         ?? throw new InvalidOperationException("No target reached!");

            return Convert.ToInt64(target.X * 100 + target.Y);
        }

        /// <summary>
        /// Fires laser round clockwise, 
        /// </summary>
        public Vector? FireLaserRoundUntilTargetReached(Vector ourAsteroid, Vector[] allOtherAsteroids, int targetNumber = 200)
        {
            var aliveAsteroids = allOtherAsteroids
                .Select(asteroid =>
                {
                    var laserVector = asteroid - ourAsteroid;
                    return new
                    {
                        angle = Vector.AngleBetween(Vector.UpNormal, laserVector.Normal),
                        laserVector,
                        distance = laserVector.Length,
                        location = asteroid
                    };
                })
                .OrderBy(asteroid => asteroid.angle)
                .ThenBy(asteroid => asteroid.distance)
                .ToList();

            var asteroidNumber = 0;

            while (aliveAsteroids.Count > 0)
            {
                var thisRound = aliveAsteroids.ToArray();
                var previousHitAngle = -0.1d;

                foreach (var asteroid in thisRound)
                {
                    if (asteroid.angle > previousHitAngle)
                    {
                        // Hit!
                        aliveAsteroids.Remove(asteroid);
                        previousHitAngle = asteroid.angle;

                        if (++asteroidNumber == targetNumber)
                        {
                            return asteroid.location;
                        }
                    }
                }
            }

            return null;
        }
    }
}
