using System.IO;

namespace Common
{
    public class InputLoaderReadAllLines : InputLoader<string[]>
    {
        public InputLoaderReadAllLines(int dayNumber)
        {
            DayNumber = dayNumber;
        }

        /// <summary>
        /// Warning: be sure set Day Number manually!
        /// </summary>
        internal InputLoaderReadAllLines()
        {
        }

        public override string[] LoadInput()
        {
            using var _ = new TimingBlock("Load input as lines");
            return File.ReadAllLines(GetInputFilePathForCurrentDayRelativeToBin());
        }
    }
}
