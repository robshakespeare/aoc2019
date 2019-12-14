using AoC.Day15;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day15
{
    public class Day15SolverTests
    {
        private readonly Day15Solver sut = new Day15Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(null);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(null);
        }
    }
}
