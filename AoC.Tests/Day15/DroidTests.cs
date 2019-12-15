using AoC.Day15;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day15
{
    public class DroidTests
    {
        private readonly Droid sut = Droid.Create();

        [Test]
        public void Test()
        {
            // ACT
            var result = sut.ExploreAndSolve();

            // ASSERT
            result.numOfStepsToReachOxygenSystem.Should().Be(366);
        }
    }
}
