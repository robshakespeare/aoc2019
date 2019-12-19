using AoC.Day20;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day20
{
    public class Day20SolverTests
    {
        private readonly Day20Solver sut = new Day20Solver();

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
