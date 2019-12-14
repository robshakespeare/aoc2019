using AoC.Day13;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day13
{
    public class Day13SolverTests
    {
        private readonly Day13Solver sut = new Day13Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(304);
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
