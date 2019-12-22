using AoC.Day21;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day21
{
    public class Day21SolverTests
    {
        private readonly Day21Solver sut = new Day21Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(19350938);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(1142986901);
        }
    }
}
