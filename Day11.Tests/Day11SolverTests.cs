using FluentAssertions;
using NUnit.Framework;

namespace Day11.Tests
{
    public class Day11SolverTests
    {
        private readonly Day11Solver sut = new Day11Solver();

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

        /*
        [Test]
        public void ExampleTest()
        {
            const string input = @"";

            // ACT
            var result = sut.SolvePart1(input.ReadAllLines());

            // ASSERT
            result.Should().Be(xyz);
        }
        */
    }
}
