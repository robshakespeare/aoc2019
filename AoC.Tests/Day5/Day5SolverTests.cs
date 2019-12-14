using AoC.Day5;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day5
{
    public class Day5SolverTests
    {
        [Test]
        public void Solve_Part1_Returns_AlreadyVerifiedResult()
        {
            const int alreadyVerifiedResult = 9219874;

            // ACT
            var result = new Day5Solver().SolvePart1();

            // ASSERT
            result.Should().Be(alreadyVerifiedResult);
        }

        [Test]
        public void Solve_Part2_Returns_AlreadyVerifiedResult()
        {
            const int alreadyVerifiedResult = 5893654;

            // ACT
            var result = new Day5Solver().SolvePart2();

            // ASSERT
            result.Should().Be(alreadyVerifiedResult);
        }
    }
}
