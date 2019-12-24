using AoC.Day24;
using Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day24
{
    public class Day24SolverTests
    {
        private readonly Day24Solver sut = new Day24Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(28903899);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(1896);
        }

        [Test]
        public void Part2_ExampleScenario()
        {
            const string input = @"....#
#..#.
#.?##
..#..
#....";

            // ACT
            var result = sut.SolvePart2(input, 10);

            // ASSERT
            result.CountInfestedTiles().Should().Be(99);

            result.Render().Should().Be(@"Depth -5:
..#..
.#.#.
..?.#
.#.#.
..#..

Depth -4:
...#.
...##
..?..
...##
...#.

Depth -3:
#.#..
.#...
..?..
.#...
#.#..

Depth -2:
.#.##
....#
..?.#
...##
.###.

Depth -1:
#..##
...##
..?..
...#.
.####

Depth 0:
.#...
.#.##
.#?..
.....
.....

Depth 1:
.##..
#..##
..?.#
##.##
#####

Depth 2:
###..
##.#.
#.?..
.#.##
#.#..

Depth 3:
..###
.....
#.?..
#....
#...#

Depth 4:
.###.
#..#.
#.?..
##.#.
.....

Depth 5:
####.
#..#.
#.?#.
####.
.....".NormalizeLineEndings());
        }
    }
}
