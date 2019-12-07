using FluentAssertions;
using NUnit.Framework;
using Common.IntCodes;

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
            result.LastOutputValue.Should().Be(testInputSystemId);
        }

        [Test]
        public void BasicParameterModesTest()
        {
            // ACT
            var result = sut.ParseAndEvaluate(
                "1002,4,3,4,33",
                default);

            // ASSERT
            result[4].Should().Be(99);
            result.LastOutputValue.Should().BeNull();
        }

        [Test]
        public void IntegersCanBeNegative()
        {
            // ACT
            var result = sut.ParseAndEvaluate(
                "1101,100,-1,4,0",
                default);

            // ASSERT
            result[4].Should().Be(99);
            result.LastOutputValue.Should().BeNull();
        }
    }
}
