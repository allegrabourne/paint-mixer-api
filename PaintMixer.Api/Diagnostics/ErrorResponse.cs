namespace PaintMixer.Api.Diagnostics
{
    public sealed class ErrorResponse
    {
        public string Code { get; init; } = string.Empty;

        public string Message { get; init; } = string.Empty;

        public string? CorrelationId { get; init; }
    }
}
