namespace Day14
{
    public struct ChemicalQuantity
    {
        public int Quantity { get; }
        public string Chemical { get; }

        public ChemicalQuantity(int quantity, string chemical)
        {
            Quantity = quantity;
            Chemical = chemical;
        }

        public override string ToString() => $"{Quantity} {Chemical}";
    }
}
