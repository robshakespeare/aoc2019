namespace Day14
{
    public struct ChemicalInput
    {
        public int Quantity { get; }
        public string Chemical { get; }

        public ChemicalInput(int quantity, string chemical)
        {
            Quantity = quantity;
            Chemical = chemical;
        }

        public override string ToString() => $"{Quantity} {Chemical}";
    }
}
