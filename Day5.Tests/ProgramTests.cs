using FluentAssertions;
using NUnit.Framework;

namespace Day5.Tests
{
    public class ProgramTests
    {
        [Test]
        public void Solve_Part1_Returns_AlreadyVerifiedResult()
        {
            const int alreadyVerifiedResult = 9219874;

            // ACT
            var result = new Program().SolvePart1();

            // ASSERT
            result.Should().Be(alreadyVerifiedResult);
        }

        [Test]
        public void Solve_Part2_Returns_AlreadyVerifiedResult()
        {
            const int alreadyVerifiedResult = 5893654;

            // ACT
            var result = new Program().SolvePart2();

            // ASSERT
            result.Should().Be(alreadyVerifiedResult);
        }
    }
}
