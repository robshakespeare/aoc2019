using AoC.DayXXX;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.DayXXX
{
    public class DayXXXSolverTests
    {
        private readonly DayXXXSolver sut = new DayXXXSolver();

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
