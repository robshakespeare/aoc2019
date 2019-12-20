using System;
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
            return File.ReadAllText(GetInputFilePathForCurrentDayRelativeToBin()).TrimEnd();
        }

        public string LoadInputSeparatePart2()
        {
            using var _ = new TimingBlock("Load separate part 2 input as text");

            var filePath = Path.Combine(
                Path.GetDirectoryName(GetInputFilePathForCurrentDayRelativeToBin()) ?? throw new InvalidOperationException(),
                "input-part-2.txt");

            return File.ReadAllText(filePath).TrimEnd();
        }
    }
}
