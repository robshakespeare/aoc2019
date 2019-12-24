using System;
using System.Linq;
using AoC.Day22;
using Common;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day22
{
    public class CardIndexShufflerTests
    {
        private static readonly CardShuffler CardShuffler = new CardShuffler(Day22Solver.Part1FactoryOrderNumber);
        private static readonly CardIndexShuffler Sut = new CardIndexShuffler(Day22Solver.Part1FactoryOrderNumber);

        [Test]
        public void TestCase1_V1_And_V2_WithMultipleIterations_ProduceSameResult()
        {
            var shuffleProcess = new ShuffleProcessParser().Parse(new InputLoaderReadAllText(22).LoadInput());
            const int numIterations = 5;

            // v1
            int v1IndexResult;
            {
                int[] deck = null;
                for (var i = 0; i < numIterations; i++)
                {
                    deck = deck == null
                        ? CardShuffler.Shuffle(shuffleProcess)
                        : CardShuffler.Shuffle(deck, shuffleProcess);
                }

                v1IndexResult = (deck ?? throw new InvalidOperationException())
                    .Select((card, index) => (card, index))
                    .Single(x => x.card == Day22Solver.Part1CardNumber)
                    .index;
            }

            // v2
            int v2IndexResult;
            {
                v2IndexResult = Day22Solver.Part1CardNumber;

                for (var i = 0; i < numIterations; i++)
                {
                    v2IndexResult = Sut.ShuffleIndex(v2IndexResult, shuffleProcess);
                }
            }

            // ASSERT
            v1IndexResult.Should().Be(v2IndexResult);
        }
    }
}
