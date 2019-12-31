namespace AoC.Day22
{
    public interface ICardShuffler
    {
        long ShuffleThenGetIndexOfCard(
            (Technique technique, int operand)[] shuffleProcess,
            long deckSize,
            long numOfShuffles,
            long cardNumber);

        long ShuffleThenGetCardAtIndex(
            (Technique technique, int operand)[] shuffleProcess,
            long deckSize,
            long numOfShuffles,
            long cardIndex);
    }
}