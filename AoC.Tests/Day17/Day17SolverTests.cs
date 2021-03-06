using AoC.Day17;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day17
{
    public class Day17SolverTests
    {
        private readonly Day17Solver sut = new Day17Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(4408);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(862452);
        }
    }
}
