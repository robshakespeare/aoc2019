using FluentAssertions;
using NUnit.Framework;

namespace Day5.Tests
{
    public class ProgramTests
    {
        [Test]
        public void Solve_Returns_AlreadyVerifiedResult()
        {
            const int alreadyVerifiedResult = 5893654;

            // ACT
            var result = Program.Solve();

            // ASSERT
            result.Should().Be(alreadyVerifiedResult);
        }
    }
}
