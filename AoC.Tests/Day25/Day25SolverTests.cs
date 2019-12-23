using AoC.Day25;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day25
{
    public class Day25SolverTests
    {
        private readonly Day25Solver sut = new Day25Solver();

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
