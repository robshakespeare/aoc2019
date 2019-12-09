namespace Common.IntCodes
{
    public enum ParameterMode
    {
        /// <summary>
        /// Positional means its value is the address
        /// </summary>
        Positional,

        /// <summary>
        /// Immediate means the value is what is used
        /// </summary>
        Immediate,

        /// <summary>
        /// Relative mode, behaves very similarly to parameters in position mode: the parameter is interpreted as a position.
        /// However, relative mode parameters don't count from address 0. Instead, they count from a value called the "relative
        /// base". The relative base starts at 0.
        /// </summary>
        Relative
    }
}
