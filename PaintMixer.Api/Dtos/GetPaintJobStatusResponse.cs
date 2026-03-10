using PaintMixer.Domain.Enums;

namespace PaintMixer.Api.Dtos
{
    public sealed class GetPaintJobStatusResponse
    {
        public short JobCode { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
