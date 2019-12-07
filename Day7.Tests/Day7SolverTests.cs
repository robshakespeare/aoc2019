using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Common.IntCodes;
using FluentAssertions;
using NUnit.Framework;

namespace Day7.Tests
{
    public class Day7SolverTests
    {
        private readonly Day7Solver sut = new Day7Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(255590);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(58285150);
        }

        /// <remarks>
        /// First version of the TryPhaseSettingSequence method, before the full implementation of the feedback loop logic!
        /// Just kept here, just in case its needed again!
        /// </remarks>
        private static int TryPhaseSettingSequenceV1(string inputProgram, IEnumerable<int> phaseSettingSequence)
        {
            var intCodeComputer = new IntCodeComputer();
            var signal = 0;

            foreach (var phaseSetting in phaseSettingSequence)
            {
                var result = intCodeComputer.ParseAndEvaluate(inputProgram, phaseSetting, signal);

                if (result.LastOutputValue == null)
                {
                    throw new InvalidOperationException("Invalid IntCodeComputer result, expected a LastOutputValue.");
                }

                signal = result.LastOutputValue.Value;
            }

            return signal;
        }

        [TestCaseSource(nameof(TryPhaseSettingSequenceTestCases))]
        public void TryPhaseSettingSequence_Tests(string inputProgram, int[] inputPhaseSettingSequence, int expectedResultMaxThrusterSignal)
        {
            // ACT
            var result = TryPhaseSettingSequenceV1(inputProgram, inputPhaseSettingSequence);

            // ASSERT
            result.Should().Be(expectedResultMaxThrusterSignal);
        }

        [TestCaseSource(nameof(TryPhaseSettingSequenceTestCases))]
        public void ParseAndEvaluateWithPhaseSettingSequenceAndFeedbackLoop_Part1_Tests(string inputProgram, int[] inputPhaseSettingSequence,
            int expectedResultMaxThrusterSignal)
        {
            var intCodeComputer = new IntCodeComputer();

            // ACT
            var result = intCodeComputer.ParseAndEvaluateWithPhaseSettingSequenceAndFeedbackLoop(inputProgram, inputPhaseSettingSequence);

            // ASSERT
            result.Should().Be(expectedResultMaxThrusterSignal);
        }

        public static IEnumerable<TestCaseData> TryPhaseSettingSequenceTestCases() => new[]
        {
            new TestCaseData(
                "3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0",
                new[] {4, 3, 2, 1, 0},
                43210),
            new TestCaseData("3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0",
                new[] {0, 1, 2, 3, 4},
                54321),
            new TestCaseData(
                "3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0",
                new[] {1, 0, 4, 3, 2},
                65210)
        };

        private static void ValidateAllPossibleCombinationsAreUnique(IEnumerable<int[]> combinations)
        {
            var lines = combinations.Select(x => string.Join(",", x)).ToArray();
            TestContext.WriteLine(string.Join(Environment.NewLine, lines));
            lines.Should().OnlyHaveUniqueItems();
        }

        [Test]
        public void GetAllPossibleCombinations_SingleValueTest()
        {
            // ACT
            var result = sut.GetAllPossibleCombinations(new[] {12});

            // ASSERT
            result.Should().BeEquivalentTo(
                new object[]
                {
                    new[] {12}
                });
            ValidateAllPossibleCombinationsAreUnique(result);
        }

        [Test]
        public void GetAllPossibleCombinations_TwoValuesTest()
        {
            // ACT
            var result = sut.GetAllPossibleCombinations(new[] {12, 64});

            // ASSERT
            result.Should().BeEquivalentTo(
                new[] {12, 64},
                new[] {64, 12});
            ValidateAllPossibleCombinationsAreUnique(result);
        }

        [Test]
        public void GetAllPossibleCombinations_ThreeValuesTest()
        {
            // ACT
            var result = sut.GetAllPossibleCombinations(new[] {1, 2, 3});

            // ASSERT
            result.Should().BeEquivalentTo(
                new[] {1, 2, 3},
                new[] {1, 3, 2},
                new[] {2, 1, 3},
                new[] {2, 3, 1},
                new[] {3, 1, 2},
                new[] {3, 2, 1});
            ValidateAllPossibleCombinationsAreUnique(result);
        }

        [Test]
        public void GetAllPossibleCombinations_FiveValues_RealWorldTest()
        {
            // ACT
            var result = sut.GetAllPossibleCombinations((..5).ToArray());

            // ASSERT
            result.Length.Should().Be(120); // i.e. 5 factorial
            ValidateAllPossibleCombinationsAreUnique(result);
        }

        [Test]
        public void GetAllPossibleCombinations_FiveValues_RealWorldTest2()
        {
            // ACT
            var result = sut.GetAllPossibleCombinations((5..10).ToArray());

            // ASSERT
            result.Length.Should().Be(120); // i.e. 5 factorial
            ValidateAllPossibleCombinationsAreUnique(result);
        }

        [Test]
        public void GetAllPossibleCombinations_SixValuesTest()
        {
            // ACT
            var result = sut.GetAllPossibleCombinations((10..16).ToArray());

            // ASSERT
            result.Length.Should().Be(720); // i.e. 6 factorial
            ValidateAllPossibleCombinationsAreUnique(result);
        }

        [TestCaseSource(nameof(ParseAndEvaluateWithPhaseSettingSequenceAndFeedbackLoopPart2TestCases))]
        public void ParseAndEvaluateWithPhaseSettingSequenceAndFeedbackLoop_Part2_Tests(string inputProgram, int[] inputPhaseSettingSequence,
            int expectedResultMaxThrusterSignal)
        {
            var intCodeComputer = new IntCodeComputer();

            // ACT
            var result = intCodeComputer.ParseAndEvaluateWithPhaseSettingSequenceAndFeedbackLoop(inputProgram, inputPhaseSettingSequence);

            // ASSERT
            result.Should().Be(expectedResultMaxThrusterSignal);
        }

        public static IEnumerable<TestCaseData> ParseAndEvaluateWithPhaseSettingSequenceAndFeedbackLoopPart2TestCases() => new[]
        {
            new TestCaseData(
                "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5",
                new[] {9, 8, 7, 6, 5},
                139629729),
            new TestCaseData(
                "3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10",
                new[] {9, 7, 8, 5, 6},
                18216)
        };
    }
}
