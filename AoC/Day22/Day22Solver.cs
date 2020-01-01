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
            const long deckSize = 119315717514047;
            const long numOfShuffles = 101741582076661;
            const int cardIndex = 2020;
            var shuffleProcess = new ShuffleProcessParser().Parse(input);

            return new CardShufflerV2().ShuffleThenGetCardAtIndex(
                shuffleProcess,
                deckSize,
                numOfShuffles,
                cardIndex);
        }
    }
}
