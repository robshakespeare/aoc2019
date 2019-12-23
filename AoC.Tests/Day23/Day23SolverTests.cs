using AoC.Day23;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day23
{
    public class Day23SolverTests
    {
        private readonly Day23Solver sut = new Day23Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(16660);
        }

        [Test]
        [Ignore("takes too long")]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(11504);
        }
    }
}
