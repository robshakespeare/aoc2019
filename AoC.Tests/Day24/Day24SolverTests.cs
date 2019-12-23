using AoC.Day24;
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
