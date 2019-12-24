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
        private delegate int IndexShuffle(
            int factoryOrderNumber,
            int numIterations,
            int cardNumber,
            (InstructionType instruction, int operand)[] shuffleProcess);

        private static int IndexShuffleV1(
            int factoryOrderNumber,
            int numIterations,
            int cardNumber,
            (InstructionType instruction, int operand)[] shuffleProcess)
        {
            var cardShuffler = new CardShuffler(factoryOrderNumber);

            int[] deck = null;
            for (var i = 0; i < numIterations; i++)
            {
                deck = cardShuffler.Shuffle(deck, shuffleProcess);
            }

            return (deck ?? throw new InvalidOperationException())
                .Select((card, index) => (card, index))
                .Single(x => x.card == cardNumber)
                .index;
        }

        private static int IndexShuffleV2(
            int factoryOrderNumber,
            int numIterations,
            int cardNumber,
            (InstructionType instruction, int operand)[] shuffleProcess)
        {
            var cardIndexShuffler = new CardIndexShuffler(factoryOrderNumber);
            var index = cardNumber;

            for (var i = 0; i < numIterations; i++)
            {
                index = cardIndexShuffler.ShuffleIndex(index, shuffleProcess);
            }

            return index;
        }

        [Test]
        public void TestCase1_V1_And_V2_WithMultipleIterations_ProduceSameResult()
        {
            var shuffleProcess = new ShuffleProcessParser().Parse(new InputLoaderReadAllText(22).LoadInput());
            const int numIterations = 5;

            IndexShuffle[] shufflers = {IndexShuffleV1, IndexShuffleV2};

            // ACT
            var results = shufflers
                .Select(shuffler => shuffler(
                    Day22Solver.Part1FactoryOrderNumber,
                    numIterations,
                    Day22Solver.Part1CardNumber,
                    shuffleProcess))
                .ToArray();

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }

            // ASSERT
            results.Length.Should().Be(2);
            results.Should().AllBeEquivalentTo(results.First());

            shufflers.Should().OnlyHaveUniqueItems();
        }
    }
}
