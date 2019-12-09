using System;
using Common.IntCodes;
using FluentAssertions;
using NUnit.Framework;

namespace Day9.Tests
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
            // rs-todo: fix!!

            // ACT
            var result = sut.ParseAndEvaluate(
                "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99");

            LogOutputs(result);

            // ASSERT
            result.LastOutputValue.Should().Be(12);
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
            result.LastOutputValue.Should().Be(0L);
        }
    }
}
