using System;
using System.Diagnostics;

namespace Common
{
    public abstract class Solver<TInput>
    {
        private Lazy<TInput> Input { get; }

        protected Solver()
        {
            Input = new Lazy<TInput>(LoadInput);
        }

        public void Run()
        {
            if (!Input.IsValueCreated)
            {
                var input = Input.Value;
                Trace.WriteLine("Ensured input was loaded. Input type is " + input?.GetType());
            }

            SolvePartTimed(1, SolvePart1);
            SolvePartTimed(2, SolvePart2);
        }

        private static void SolvePartTimed(int partNum, Func<int?> solver)
        {
            using var _ = new TimingBlock($"Part {partNum}");
            var result = solver();
            Console.Write($"Part {partNum}: ");
            ColorConsole.WriteLine(result, ConsoleColor.Green);
        }

        public int? SolvePart1() => SolvePart1(Input.Value);

        public int? SolvePart2() => SolvePart2(Input.Value);

        public virtual int? SolvePart1(TInput input)
        {
            Console.WriteLine("Part 1 not yet implemented");
            return null;
        }

        public virtual int? SolvePart2(TInput input)
        {
            Console.WriteLine("Part 2 not yet implemented");
            return null;
        }

        protected abstract TInput LoadInput();
    }
}
