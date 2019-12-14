namespace Day14
{
    public struct ChemicalOutput
    {
        public int Quantity { get; }
        public string Chemical { get; }

        public ChemicalOutput(int quantity, string chemical)
        {
            Quantity = quantity;
            Chemical = chemical;
        }

        public override string ToString() => $"{Quantity} {Chemical}";
    }
}
