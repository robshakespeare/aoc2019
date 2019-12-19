using AoC.Day19;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day19
{
    public class Day19SolverTests
    {
        private readonly Day19Solver sut = new Day19Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(220);
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
