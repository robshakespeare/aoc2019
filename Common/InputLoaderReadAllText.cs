using System.IO;

namespace Common
{
    public class InputLoaderReadAllText : InputLoader<string>
    {
        public InputLoaderReadAllText(int dayNumber)
        {
            DayNumber = dayNumber;
        }

        /// <summary>
        /// Warning: be sure set Day Number manually!
        /// </summary>
        internal InputLoaderReadAllText()
        {
        }

        public override string LoadInput()
        {
            using var _ = new TimingBlock("Load input as text");
            return File.ReadAllText(GetInputFilePathForCurrentDayRelativeToBin());
        }
    }
}
