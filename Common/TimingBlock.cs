using System;
using System.Diagnostics;

namespace Common
{
    public class TimingBlock : IDisposable
    {
        private readonly string _name;
        private readonly Stopwatch _stopwatch;

        public TimingBlock(string name)
        {
            _name = name;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            Console.WriteLine($"[{_name}] time taken (seconds): {_stopwatch.Elapsed.TotalSeconds:0.000}");
        }
    }
}
