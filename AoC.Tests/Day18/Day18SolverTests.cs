using AoC.Day18;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day18
{
    public class Day18SolverTests
    {
        private readonly Day18Solver sut = new Day18Solver();

        [Test]
        public void Part1_TestCase1()
        {
            // ACT
            var result = sut.SolvePart1(@"#########
#b.A.@.a#
#########");

            // ASSERT
            result.Should().Be(8);
        }

        [Test]
        public void Part1_TestCase2()
        {
            // ACT
            var result = sut.SolvePart1(@"########################
#f.D.E.e.C.b.A.@.a.B.c.#
######################.#
#d.....................#
########################");

            // ASSERT
            result.Should().Be(86);
        }

        [Test]
        public void Part1_TestCase3()
        {
            // ACT
            var result = sut.SolvePart1(@"########################
#...............b.C.D.f#
#.######################
#.....@.a.B.c.d.A.e.F.g#
########################");

            // ASSERT
            result.Should().Be(132);
        }

        [Test]
        public void Part1_TestCase4()
        {
            // ACT
            var result = sut.SolvePart1(@"#################
#i.G..c...e..H.p#
########.########
#j.A..b...f..D.o#
########@########
#k.E..a...g..B.n#
########.########
#l.F..d...h..C.m#
#################");

            // ASSERT
            result.Should().Be(136);
        }

        [Test]
        public void Part1_TestCase5()
        {
            // ACT
            var result = sut.SolvePart1(@"########################
#@..............ac.GI.b#
###d#e#f################
###A#B#C################
###g#h#i################
########################");

            // ASSERT
            result.Should().Be(81);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(null);
        }

        ////[Test] // rs-todo: Part2ReTest
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(null);
        }
    }
}
