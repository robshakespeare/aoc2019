using System;
using System.Diagnostics;

namespace Common
{
    public class TimingBlock : IDisposable
    {
        private readonly Stopwatch _stopwatch;

        public TimingBlock()
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            Console.WriteLine($"Time taken (seconds): {_stopwatch.Elapsed.TotalSeconds:0.###}");
        }
    }
}
