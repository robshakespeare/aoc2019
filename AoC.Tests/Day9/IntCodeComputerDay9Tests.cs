using Common.IntCodes;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day9
{
    public class IntCodeComputerDay9Tests
    {
        private readonly IntCodeComputer sut = new IntCodeComputer();

        private static void LogOutputs(IntCodeState result)
        {
            TestContext.WriteLine(string.Join(",", result.Outputs));
        }

        [Test]
        public void TakesNoInputAndProducesACopyOfItselfAsOutput()
        {
            const string inputProgram = "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99";

            // ACT
            var result = sut.ParseAndEvaluate(inputProgram);

            LogOutputs(result);

            // ASSERT
            var outputs = string.Join(",", result.Outputs);

            outputs.Should().Be(inputProgram);
        }

        [Test]
        public void ShouldOutputA16DigitNumber()
        {
            // ACT
            var result = sut.ParseAndEvaluate(
                "1102,34915192,34915192,7,4,7,99,0");

            LogOutputs(result);

            // ASSERT
            result.LastOutputValue.Should().Be(1219070632396864L);
        }

        [Test]
        public void ShouldOutputTheLargeNumberInTheMiddle()
        {
            // ACT
            var result = sut.ParseAndEvaluate(
                "104,1125899906842624,99");

            LogOutputs(result);

            // ASSERT
            result.LastOutputValue.Should().Be(1125899906842624L);
        }
    }
}
