namespace AoC.Day24
{
    public class GridceptionSpec
    {
        public Vector Center { get; }
        public Vector TopLeft { get; }
        public Vector TopRight { get; }
        public Vector BottomLeft { get; }
        public Vector BottomRight { get; }

        public GridceptionSpec(Vector center, int top, int left, int right, int bottom)
        {
            Center = center;
            TopLeft = new Vector(left, top);
            TopRight = new Vector(right, top);
            BottomLeft = new Vector(left, bottom);
            BottomRight = new Vector(right, bottom);
        }
    }
}
