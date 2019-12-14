using AoC.Day12;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day12
{
    public class SimulatorTests
    {
        [Test]
        public void TestCase1_SingleSteps_CheckPositionAndVelocity_AreCalculatedAsExpected()
        {
            var sut = new Simulator(@"<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>");

            // ASSERT - Step 0
            {
                var text = sut.BuildPositionAndVelocityText();
                text.Should().Be(@"pos=<x=-1, y=  0, z= 2>, vel=<x= 0, y= 0, z= 0>
pos=<x= 2, y=-10, z=-7>, vel=<x= 0, y= 0, z= 0>
pos=<x= 4, y= -8, z= 8>, vel=<x= 0, y= 0, z= 0>
pos=<x= 3, y=  5, z=-1>, vel=<x= 0, y= 0, z= 0>".NormalizePositionAndVelocityText());
            }

            // ACT & ASSERT - Step 1
            {
                sut.Update();
                var text = sut.BuildPositionAndVelocityText();
                text.Should().Be(@"pos=<x= 2, y=-1, z= 1>, vel=<x= 3, y=-1, z=-1>
pos=<x= 3, y=-7, z=-4>, vel=<x= 1, y= 3, z= 3>
pos=<x= 1, y=-7, z= 5>, vel=<x=-3, y= 1, z=-3>
pos=<x= 2, y= 2, z= 0>, vel=<x=-1, y=-3, z= 1>".NormalizePositionAndVelocityText());
            }

            // ACT & ASSERT - Step 2
            {
                sut.Update();
                var text = sut.BuildPositionAndVelocityText();
                text.Should().Be(@"pos=<x= 5, y=-3, z=-1>, vel=<x= 3, y=-2, z=-2>
pos=<x= 1, y=-2, z= 2>, vel=<x=-2, y= 5, z= 6>
pos=<x= 1, y=-4, z=-1>, vel=<x= 0, y= 3, z=-6>
pos=<x= 1, y=-4, z= 2>, vel=<x=-1, y=-6, z= 2>".NormalizePositionAndVelocityText());
            }

            // ACT & ASSERT - Step 3
            {
                sut.Update();
                var text = sut.BuildPositionAndVelocityText();
                text.Should().Be(@"pos=<x= 5, y=-6, z=-1>, vel=<x= 0, y=-3, z= 0>
pos=<x= 0, y= 0, z= 6>, vel=<x=-1, y= 2, z= 4>
pos=<x= 2, y= 1, z=-5>, vel=<x= 1, y= 5, z=-4>
pos=<x= 1, y=-8, z= 2>, vel=<x= 0, y=-4, z= 0>".NormalizePositionAndVelocityText());
            }

            // ACT & ASSERT - Step 4
            {
                sut.Update();
                var text = sut.BuildPositionAndVelocityText();
                text.Should().Be(@"pos=<x= 2, y=-8, z= 0>, vel=<x=-3, y=-2, z= 1>
pos=<x= 2, y= 1, z= 7>, vel=<x= 2, y= 1, z= 1>
pos=<x= 2, y= 3, z=-6>, vel=<x= 0, y= 2, z=-1>
pos=<x= 2, y=-9, z= 1>, vel=<x= 1, y=-1, z=-1>".NormalizePositionAndVelocityText());
            }

            // ACT & ASSERT - Step 4
            {
                sut.Update();
                var text = sut.BuildPositionAndVelocityText();
                text.Should().Be(@"pos=<x=-1, y=-9, z= 2>, vel=<x=-3, y=-1, z= 2>
pos=<x= 4, y= 1, z= 5>, vel=<x= 2, y= 0, z=-2>
pos=<x= 2, y= 2, z=-4>, vel=<x= 0, y=-1, z= 2>
pos=<x= 3, y=-7, z=-1>, vel=<x= 1, y= 2, z=-2>".NormalizePositionAndVelocityText());
            }
        }

        [Test]
        public void FindFirstRepeatingStateStepNumber_Test1()
        {
            var sut = new Simulator(@"<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>");

            // ACT
            var result = sut.FindFirstRepeatingStateStepNumber();

            // ASSERT
            result.Should().Be(2772);
        }

        [Test]
        public void FindFirstRepeatingStateStepNumber_Test2()
        {
            var sut = new Simulator(@"<x=-8, y=-10, z=0>
<x=5, y=5, z=10>
<x=2, y=-7, z=3>
<x=9, y=-8, z=-3>");

            // ACT
            var result = sut.FindFirstRepeatingStateStepNumber();

            // ASSERT
            result.Should().Be(4686774924);
        }
    }
}
