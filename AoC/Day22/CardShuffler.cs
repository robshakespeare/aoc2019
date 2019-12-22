using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Common.Extensions;

namespace AoC.Day22
{
    public class CardShuffler
    {
        private readonly int factoryOrderNumber;

        public CardShuffler(in int factoryOrderNumber)
        {
            this.factoryOrderNumber = factoryOrderNumber;
        }

        public IReadOnlyList<int> Shuffle(in string completeShuffleProcessInput)
        {
            var factoryOrderDeck = Enumerable.Range(0, factoryOrderNumber).Select(n => (int?)n).ToReadOnlyArray();

            var shuffleProcessParser = new ShuffleProcessParser();
            var deck = factoryOrderDeck;

            foreach (var (instruction, operand) in shuffleProcessParser.Parse(completeShuffleProcessInput))
            {
                deck = instruction switch
                    {
                    InstructionType.DealIntoNewStack => DealIntoNewStack(deck),
                    InstructionType.CutPositive => CutPositive(deck, operand),
                    InstructionType.CutNegative => CutNegative(deck, operand),
                    InstructionType.DealWithIncrement => DealWithIncrement(deck, operand),
                    _ => throw new InvalidOperationException("Unexpected instruction type: " + instruction)
                    };
            }

            return Validate(deck).Select(card => card ?? throw new InvalidOperationException()).ToReadOnlyArray();
        }

        private IReadOnlyList<int?> DealIntoNewStack(in IReadOnlyList<int?> deck) =>
            Validate(deck.Reverse().ToReadOnlyArray());

        private IReadOnlyList<int?> CutPositive(in IReadOnlyList<int?> deck, in int numCards) =>
            Validate(deck.Skip(numCards).Concat(deck.Take(numCards)).ToReadOnlyArray());

        private IReadOnlyList<int?> CutNegative(in IReadOnlyList<int?> deck, in int numCards)
        {
            var offset = deck.Count - numCards;
            return Validate(deck.Skip(offset).Concat(deck.Take(offset)).ToReadOnlyArray());
        }

        private IReadOnlyList<int?> DealWithIncrement(in IReadOnlyList<int?> deck, in int increment)
        {
            var result = new int?[deck.Count];
            var index = 0;
            foreach (var card in deck)
            {
                if (result[index] != null)
                {
                    throw new InvalidOperationException(
                        $"Cannot deal card {card} with increment {increment}, index {index} has already been dealt to with card {result[index]}");
                }

                result[index] = card;
                index = (index + increment) % deck.Count;
            }

            return Validate(result.ToReadOnlyArray());
        }

        private IReadOnlyList<int?> Validate(
            in IReadOnlyList<int?> deck,
            [CallerMemberName] in string? callerMemberName = null)
        {
            if (deck.Count != factoryOrderNumber)
            {
                throw new InvalidOperationException($"Error in {callerMemberName}, deck count {deck.Count} should be {factoryOrderNumber}.");
            }

            foreach (var (card, index) in deck.Select((card, index) => (card, index)))
            {
                if (card == null)
                {
                    throw new InvalidOperationException($"Error in {callerMemberName}, {index} was null.");
                }
            }

            return deck;
        }
    }
}
