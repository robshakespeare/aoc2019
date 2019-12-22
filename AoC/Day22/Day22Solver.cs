using Common;

namespace AoC.Day22
{
    public class Day22Solver : SolverReadAllText
    {
        public override long? SolvePart1(string input)
        {
            var shuffledCards = new CardShuffler().Shuffle(input, 10007);
            return shuffledCards[2019];
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
