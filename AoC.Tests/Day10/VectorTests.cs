using AoC.Day10;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day10
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

        [Test]
        public void AngleBetween_ShouldBe90Degrees()
        {
            var result = Vector.AngleBetween(Vector.UpNormal, new Vector(1, 0));

            result.Should().Be(90);
        }

        [Test]
        public void AngleBetween_ShouldBe0Degrees()
        {
            var result = Vector.AngleBetween(Vector.UpNormal, Vector.UpNormal);

            result.Should().Be(0);
        }

        [Test]
        public void AngleBetween_ShouldBe180Degrees()
        {
            var result = Vector.AngleBetween(Vector.UpNormal, new Vector(0, 55));

            result.Should().Be(180);
        }

        [Test]
        public void AngleBetween_ShouldBe45Degrees()
        {
            var result = Vector.AngleBetween(Vector.UpNormal, new Vector(0.5, -0.5));

            result.Should().Be(45);
        }

        [Test]
        public void AngleBetween_ShouldBe270Degrees()
        {
            var result = Vector.AngleBetween(Vector.UpNormal, new Vector(-12, 0));

            result.Should().Be(270);
        }

        [Test]
        public void AngleBetween_ShouldBeJustAfter180Degrees()
        {
            var result = Vector.AngleBetween(Vector.UpNormal, new Vector(-0.1, 5));

            result.Should().BeGreaterThan(180);
            result.Should().BeLessThan(182);
        }

        [Test]
        public void AngleBetween_ShouldBeJustLessThan360Degrees()
        {
            var result = Vector.AngleBetween(Vector.UpNormal, new Vector(-0.1, -2));

            result.Should().BeGreaterThan(355);
            result.Should().BeLessThan(360);
        }
    }
}
