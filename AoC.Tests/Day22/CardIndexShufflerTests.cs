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

        [TestCase(Day22Solver.Part1FactoryOrderNumber, 5, Day22Solver.Part1CardNumber)]
        [TestCase(Day22Solver.Part1FactoryOrderNumber, 20, Day22Solver.Part1CardNumber)]
        [TestCase(Day22Solver.Part1FactoryOrderNumber, 5, 1234)]
        [TestCase(Day22Solver.Part1FactoryOrderNumber, 20, 1234)]
        public void TestCase1_V1_And_V2_WithMultipleIterations_ProduceSameResult(
            int factoryOrderNumber,
            int numIterations,
            int cardNumber)
        {
            var shuffleProcess = new ShuffleProcessParser().Parse(new InputLoaderReadAllText(22).LoadInput());

            IndexShuffle[] shufflers = {IndexShuffleV1, IndexShuffleV2};

            // ACT
            var results = shufflers
                .Select(shuffler => shuffler(factoryOrderNumber, numIterations, cardNumber, shuffleProcess))
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

        [TestCase(Day22Solver.Part1FactoryOrderNumber, Day22Solver.Part1CardNumber)]
        public void Test(int factoryOrderNumber, int cardNumber)
        {
            // Want to know what the "shift" is like
            var shuffleProcess = new ShuffleProcessParser().Parse(new InputLoaderReadAllText(22).LoadInput());

            foreach (var testCardNumber in new[] { cardNumber, 1234 })
            {
                foreach (var numIterations in new[] { 1, 2, 20 })
                {
                    var result = IndexShuffleV2(factoryOrderNumber, numIterations, testCardNumber, shuffleProcess);

                    var delta = result - cardNumber;
                    Console.WriteLine(new { testCardNumber, result, delta, shiftTest = delta / numIterations });
                }

                //var result = IndexShuffleV2(factoryOrderNumber, 1, testCardNumber, shuffleProcess);
                //var result3 = IndexShuffleV2(factoryOrderNumber, 2, testCardNumber, shuffleProcess);
                //var result2 = IndexShuffleV2(factoryOrderNumber, 20, testCardNumber, shuffleProcess);

                //var shift = result - testCardNumber;
                //Console.WriteLine(new { testCardNumber, result, shift, shiftTest = (result - cardNumber) / 1 });
                //Console.WriteLine(new { testCardNumber, result3, shiftTest = (result3 - cardNumber) / 2 });
                //Console.WriteLine(new { testCardNumber, result2, shiftTest = (result2 - cardNumber) / 20 });
            }
        }
    }
}
