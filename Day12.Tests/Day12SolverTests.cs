using FluentAssertions;
using NUnit.Framework;

namespace Day12.Tests
{
    public class Day12SolverTests
    {
        private readonly Day12Solver sut = new Day12Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(6678);
        }

        [Test]
        [Ignore("This test takes too long at the moment. Clearly, I might need to find a more efficient way to simulate the universe!")]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(null);
        }
    }
}
