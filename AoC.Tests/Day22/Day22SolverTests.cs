using AoC.Day22;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day22
{
    public class Day22SolverTests
    {
        private readonly Day22Solver sut = new Day22Solver();

        [Test]
        public void Part1ReTest()
        {
            const int expectedResult = 2306;

            // ACT
            var part1Result = sut.SolvePart1();
            var part1ResultV2 = sut.SolvePart1V2();

            // ASSERT
            part1Result.Should().Be(expectedResult);
            part1ResultV2.Should().Be(expectedResult);

            part1Result.Should().Be(part1ResultV2);
        }

        [Test]
        public void Part2ReTest()
        {
            const long expectedResult = 12545532223512L;

            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(expectedResult);
        }
    }
}
