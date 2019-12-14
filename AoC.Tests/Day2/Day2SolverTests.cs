using AoC.Day2;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day2
{
    public class Day2SolverTests
    {
        private readonly Day2Solver sut = new Day2Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(4484226);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(5696);
        }
    }
}
