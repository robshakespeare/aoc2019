using FluentAssertions;
using NUnit.Framework;

namespace Day12.Tests
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
        }
    }
}
