namespace AoC.Day14
{
    public class Reaction
    {
        public ChemicalQuantity Output { get; }
        public ChemicalQuantity[] Inputs { get; }

        public Reaction(ChemicalQuantity output, params ChemicalQuantity[] inputs)
        {
            Output = output;
            Inputs = inputs;
        }

        public override string ToString() => $"{string.Join(", ", Inputs)} => {Output}";
    }
}
