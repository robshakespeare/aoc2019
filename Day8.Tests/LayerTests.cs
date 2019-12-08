using FluentAssertions;
using NUnit.Framework;

namespace Day8.Tests
{
    public class LayerTests
    {
        [Test]
        public void CountNumberOfDigits_Tests()
        {
            var sut = new Layer(new [] {1,1,2,3,5,1,5,1});

            // ACT & ASSERT
            sut.CountNumberOfDigits(1).Should().Be(4);
            sut.CountNumberOfDigits(2).Should().Be(1);
            sut.CountNumberOfDigits(3).Should().Be(1);
            sut.CountNumberOfDigits(4).Should().Be(0);
            sut.CountNumberOfDigits(5).Should().Be(2);
        }
    }
}
