using FluentAssertions;
using NUnit.Framework;

namespace Day10.Tests
{
    public class VectorTests
    {
        [Test]
        public void Length_OfVector_Test()
        {
            var sut = new Vector(6, 8);

            sut.Length.Should().Be(10);
        }

        [Test]
        public void Length_OfNegativeVector_Test()
        {
            new Vector(-6, -8).Length.Should().Be(10);
            new Vector(6, -8).Length.Should().Be(10);
            new Vector(-6, 8).Length.Should().Be(10);
        }

        [Test]
        public void Normal_OfVector_Test()
        {
            var sut1 = new Vector(4, 4);
            var sut2 = new Vector(2, 2);

            Assert.AreEqual(sut1.Normal, sut2.Normal);
        }

        [Test]
        public void Normal_OfVector_Test2()
        {
            var sut1 = new Vector(1, 3);
            var sut2 = new Vector(2, 6);

            Assert.AreEqual(sut1.Normal, sut2.Normal);
        }

        [Test]
        public void Normal_OfVector_WhenExactlyInLine()
        {
            var sut1 = new Vector(1, 3);
            var sut2 = new Vector(3, 6);

            Assert.AreNotEqual(sut1.Normal, sut2.Normal);
        }
    }
}
