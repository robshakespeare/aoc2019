using System.Linq;
using Common;

namespace AoC.Day22
{
    public class Day22Solver : SolverReadAllText
    {
        private const int Part1FactoryOrderNumber = 10007;
        private const int Part1CardNumber = 2019; 

        public override long? SolvePart1(string input)
        {
            var parser = new ShuffleProcessParser();
            var shuffledCards = new CardShuffler(Part1FactoryOrderNumber).Shuffle(parser.Parse(input));
            return shuffledCards
                .Select((card, index) => (card, index))
                .Single(x => x.card == Part1CardNumber)
                .index;
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
