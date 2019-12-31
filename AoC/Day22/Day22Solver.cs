using Common;

namespace AoC.Day22
{
    public class Day22Solver : SolverReadAllText
    {
        public const int Part1DeckSize = 10007;
        public const int Part1CardNumber = 2019; 

        public override long? SolvePart1(string input)
        {
            var shuffleProcess = new ShuffleProcessParser().Parse(input);
            return new CardShuffler().ShuffleThenGetIndexOfCard(
                shuffleProcess,
                Part1DeckSize,
                1,
                Part1CardNumber);
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
