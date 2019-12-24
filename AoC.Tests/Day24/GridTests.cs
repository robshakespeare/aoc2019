using AoC.Day24;
using Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day24
{
    public class GridTests
    {
        [Test]
        public void Load_And_Render_Roundtrip_Test()
        {
            var input = @"#....
####.
...##
#.##.
.##.#".NormalizeLineEndings();

            var sut = Grid.Load(input);

            // ACT
            var result = sut.Render();

            // ASSERT
            result.Should().Be(input);
        }

        [Test]
        public void CalculateBiodiversityRating_TestCase1()
        {
            var sut = Grid.Load(@".....
.....
.....
#....
.#...");
            // ACT
            var result = sut.CalculateBiodiversityRating();

            // ASSERT
            result.Should().Be(2129920);
        }

        [Test]
        public void Update_Infestation_ExampleScenario()
        {
            var sut = Grid.Load(@"....#
#..#.
#..##
..#..
#....");

            // ACT & ASSERT 1
            sut.Update();
            sut.Render().Should().Be(@"#..#.
####.
###.#
##.##
.##..".NormalizeLineEndings());

            // ACT & ASSERT 2
            sut.Update();
            sut.Render().Should().Be(@"#####
....#
....#
...#.
#.###".NormalizeLineEndings());

            // ACT & ASSERT 3
            sut.Update();
            sut.Render().Should().Be(@"#....
####.
...##
#.##.
.##.#".NormalizeLineEndings());

            // ACT & ASSERT 4
            sut.Update();
            sut.Render().Should().Be(@"####.
....#
##..#
.....
##...".NormalizeLineEndings());
        }
    }
}
