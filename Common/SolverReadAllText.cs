namespace Common
{
    public abstract class SolverReadAllText : Solver<string>
    {
        protected SolverReadAllText() : base(new InputLoaderReadAllText())
        {
        }
    }
}
