namespace Common
{
    public class SolverStaticInput<TInput> : Solver<TInput>
    {
        private readonly TInput input;

        public SolverStaticInput(TInput input)
        {
            this.input = input;
        }

        protected override TInput LoadInput() => input;
    }
}
