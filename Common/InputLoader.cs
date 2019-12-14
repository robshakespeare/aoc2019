using System;
using System.IO;

namespace Common
{
    public abstract class InputLoader<TInput>
    {
        public int? DayNumber { get; internal set; }

        public abstract TInput LoadInput();

        public string GetInputFilePathForCurrentDayRelativeToBin()
        {
            if (DayNumber == null)
            {
                throw new InvalidOperationException("Day number hasn't been set");
            }

            return Path.Combine($"Day{DayNumber}", "input.txt");
        }
    }
}
