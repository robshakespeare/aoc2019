using System;
using System.Diagnostics.CodeAnalysis;

namespace Common
{
    public abstract class SolverWithInput<TInput> : SolverNoInput
    {
        private TInput Input { get; }

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor",
            Justification = "Not a problem here, there is nothing else to initialse.")]
        protected SolverWithInput()
        {
            Input = LoadInput();
        }

        public sealed override int? SolvePart1() => SolvePart1(Input);

        public sealed override int? SolvePart2() => SolvePart2(Input);

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
