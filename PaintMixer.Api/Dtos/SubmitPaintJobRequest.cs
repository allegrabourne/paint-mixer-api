namespace PaintMixer.Api.Dtos
{
    public sealed class SubmitPaintJobRequest
    {
        public int Red { get; init; }
        public int Blue { get; init; }
        public int Yellow { get; init; }
        public int White { get; init; }
        public int Black { get; init; }
        public int Green { get; init; }
    }
}
