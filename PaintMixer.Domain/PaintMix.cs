namespace PaintMixer.Domain
{
    public sealed class PaintMix
    {
        private const int MaxMixTotal = 100;

        public byte Red { get; }
        public byte Blue { get; }
        public byte Yellow { get; }
        public byte White { get; }
        public byte Black { get; }
        public byte Green { get; }

        public int Total => Red + Blue + Yellow + White + Black + Green;

        public PaintMix(byte red, byte blue, byte yellow, byte white, byte black, byte green)
        {
            var total = red + blue + yellow + white + black + green;

            if (total > MaxMixTotal)
            {
                throw new ArgumentException(
                    $"Total paint mix cannot exceed {MaxMixTotal}. Actual total: {total}. " +
                    $"Received values red={red}, blue={blue}, yellow={yellow}, white={white}, black={black}, green={green}.",
                    nameof(red));
            }

            Red = red;
            Blue = blue;
            Yellow = yellow;
            White = white;
            Black = black;
            Green = green;
        }
    }
}