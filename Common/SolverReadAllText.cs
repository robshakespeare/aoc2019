using System.IO;

namespace Common
{
    public abstract class SolverReadAllText : Solver<string>
    {
        protected override string LoadInput()
        {
            using var _ = new TimingBlock("Load input");
            return File.ReadAllText("input.txt");
        }
    }
}
