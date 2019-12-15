using AoC.Day15;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day15
{
    public class DroidTests
    {
        private readonly Droid sut = Droid.Create(true);

        [Test]
        public void Test()
        {
            // ACT
            var result = sut.ExploreAndSolve();

            // ASSERT
            result.numOfStepsToReachOxygenSystem.Should().Be(366);
            result.iterationsToFillWithOxygen.Should().Be(384);
        }
    }
}
