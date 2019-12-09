using FluentAssertions;
using NUnit.Framework;

namespace Day9.Tests
{
    public class Day9SolverTests
    {
        private readonly Day9Solver sut = new Day9Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(3518157894);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(80379);
        }
    }
}
