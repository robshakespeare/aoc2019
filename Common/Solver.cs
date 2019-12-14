using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Common
{
    public abstract class Solver<TInput, TOutputPart1, TOutputPart2>
    {
        private Lazy<TInput> Input { get; }

        protected Solver(IInputLoader<TInput> inputLoader)
        {
            Input = new Lazy<TInput>(inputLoader.LoadInput);
        }

        public void Run()
        {
            PrintTitle();

            if (!Input.IsValueCreated)
            {
                var input = Input.Value;
                Trace.WriteLine("Ensured input was loaded. Input type is " + input?.GetType());
            }

            SolvePart1();
            SolvePart2();
        }

        private static TOutput SolvePartTimed<TOutput>(int partNum, Func<TOutput> solver)
        {
            using var _ = new TimingBlock($"Part {partNum}");
            var result = solver();
            Console.Write($"Part {partNum}: ");
            ColorConsole.WriteLine(result, ConsoleColor.Green);
            return result;
        }

        public TOutputPart1 SolvePart1() => SolvePartTimed(1, () => SolvePart1(Input.Value));

        public TOutputPart2 SolvePart2() => SolvePartTimed(2, () => SolvePart2(Input.Value));

        [return: MaybeNull]
        public virtual TOutputPart1 SolvePart1(TInput input)
        {
            Console.WriteLine("Part 1 not yet implemented");
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter. Justification: We always wants solutions to be nullable, to support solve methods that have not yet been implemented
            return default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
        }

        [return: MaybeNull] 
        public virtual TOutputPart2 SolvePart2(TInput input)
        {
            Console.WriteLine("Part 2 not yet implemented");
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter. Justification: We always wants solutions to be nullable, to support solve methods that have not yet been implemented
            return default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
        }

        private void PrintTitle()
        {
            var dayNumRegex = new Regex(@"Day(?<dayNum>\d+)");
            var fullName = GetType().FullName;
            Match match;

            if (fullName != null &&
                (match = dayNumRegex.Match(fullName)).Success &&
                match.Groups["dayNum"].Success)
            {
                var title = $"Day {match.Groups["dayNum"].Value}";
                ColorConsole.WriteLine(title, ConsoleColor.DarkYellow);
            }
        }
    }

    public abstract class Solver<TInput> : Solver<TInput, long?, long?>
    {
        protected Solver(IInputLoader<TInput> inputLoader) : base(inputLoader)
        {
        }
    }
}
