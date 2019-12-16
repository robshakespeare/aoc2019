using AoC.Day16;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day16
{
    public class SignalCleanerTests
    {
        private static readonly SignalCleaner Sut = new SignalCleaner();

        [Test]
        public void Clean_TestCase1_1Phase()
        {
            // ACT
            var result = Sut.Clean("12345678", 1);

            // ASSERT
            result.Should().Be("48226158");
        }

        [Test]
        public void Clean_TestCase1_4Phases()
        {
            // ACT
            var result = Sut.Clean("12345678", 4);

            // ASSERT
            result.Should().Be("01029498");
        }

        [Test]
        public void Clean_TestCase2()
        {
            // ACT
            var result = Sut.Clean("80871224585914546619083218645595");

            // ASSERT
            result.Should().Be("24176176");
        }

        [Test]
        public void Clean_TestCase3()
        {
            // ACT
            var result = Sut.Clean("19617804207202209144916044189917");

            // ASSERT
            result.Should().Be("73745418");
        }

        [Test]
        public void Clean_TestCase4()
        {
            // ACT
            var result = Sut.Clean("69317163492948606335995924319873");

            // ASSERT
            result.Should().Be("52432133");
        }
    }
}
