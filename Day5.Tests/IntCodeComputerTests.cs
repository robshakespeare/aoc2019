using FluentAssertions;
using NUnit.Framework;

namespace Day5.Tests
{
    public class IntCodeComputerTests
    {
        private IntCodeComputer sut = new IntCodeComputer();

        [Test]
        public void ProgramThatOutputsWhateverItGetsAsInputThenHalts()
        {
            const int testInputSystemId = 123765;

            // ACT
            var result = sut.ParseAndEvaluate(
                "3,0,4,0,99",
                testInputSystemId);

            // ASSERT
            result.diagnosticCode.Should().Be(testInputSystemId);
        }
    }
}
