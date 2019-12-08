using Common;

namespace Day8
{
    public class Day8Solver : SolverReadAllText
    {
        public override int? SolvePart1(string input)
        {
            const int imageWidth = 25;
            const int imageHeight = 6;

            var image = Image.Parse(input, imageWidth, imageHeight);

            return image.GetCorruptionCheckDigit();
        }

        public override int? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
