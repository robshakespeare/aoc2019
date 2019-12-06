using System;
using System.Diagnostics;

namespace Common
{
    public class TimingBlock : IDisposable
    {
        private readonly string name;
        private readonly Stopwatch stopwatch;

        public TimingBlock(string name)
        {
            this.name = name;
            stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            stopwatch.Stop();

            ColorConsole.Write($"[{name}] time taken (seconds): {stopwatch.Elapsed.TotalSeconds:0.000}", ConsoleColor.DarkGray);

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
