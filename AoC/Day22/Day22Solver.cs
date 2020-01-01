using Common;

namespace AoC.Day22
{
    public class Day22Solver : SolverReadAllText
    {
        public override long? SolvePart1(string input)
        {
            const int deckSize = 10007;
            const int cardNumber = 2019;
            var shuffleProcess = new ShuffleProcessParser().Parse(input);

            return new CardShuffler().ShuffleThenGetIndexOfCard(
                shuffleProcess,
                deckSize,
                cardNumber);
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
