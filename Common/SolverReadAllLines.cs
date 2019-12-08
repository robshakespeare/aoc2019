namespace Common
{
    public abstract class SolverReadAllLines : Solver<string[]>
    {
        protected SolverReadAllLines() : base(new InputLoaderReadAllLines())
        {
        }
    }
}
