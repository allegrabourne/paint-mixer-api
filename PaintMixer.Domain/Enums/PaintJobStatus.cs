namespace PaintMixer.Domain.Enums
{
    public enum PaintJobStatus
    {
        Unknown = 0,
        Queued,
        Running,
        Completed,
        Cancelled,
        Failed
    }
}
