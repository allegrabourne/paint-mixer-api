using PaintMixer.Domain.Enums;

namespace PaintMixer.Domain
{
    public sealed class PaintJob
    {
        public short JobCode { get; }
        public PaintMix Mix { get; }
        public PaintJobStatus Status { get; }

        public PaintJob(short jobCode, PaintMix mix, PaintJobStatus status)
        {
            if (jobCode <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(jobCode), "Job code must be greater than zero.");
            }

            Mix = mix ?? throw new ArgumentNullException(nameof(mix));

            Status = status;

            JobCode = jobCode;
        }
    }
}
