using System.Linq;
using Common;

namespace AoC.Day22
{
    public class Day22Solver : SolverReadAllText
    {
        public override long? SolvePart1(string input)
        {
            var shuffledCards = new CardShuffler(10007).Shuffle(input);
            return shuffledCards
                .Select((card, index) => (card, index))
                .Single(x => x.card == 2019)
                .index;
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
