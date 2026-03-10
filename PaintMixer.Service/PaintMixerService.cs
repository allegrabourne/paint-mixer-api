using PaintMixer.Domain;
using PaintMixer.Domain.Enums;
using PaintMixer.Service.Interfaces;

namespace PaintMixer.Service
{
    /// <summary>
    /// PaintMixerService is the main orcehestration class that provides methods to submit paint mixing jobs, query their status, and cancel them. 
    /// It interacts with an underlying paint mixer device through the IPaintMixerDevice interface.
    /// </summary>
    public sealed class PaintMixerService(IPaintMixerDevice paintMixerDevice) : IPaintMixerService
    {
        private readonly IPaintMixerDevice _paintMixerDevice = paintMixerDevice ?? 
            throw new ArgumentNullException(nameof(paintMixerDevice));

        public Task<short> SubmitJobAsync(PaintMix mix, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(mix);

            var result = _paintMixerDevice.SubmitJob(
                red: mix.Red,
                black: mix.Black,
                white: mix.White,
                yellow: mix.Yellow,
                blue: mix.Blue,
                green: mix.Green);

            if (result < 0)
            {
                throw new InvalidOperationException("The paint mixer rejected the job submission.");
            }

            return Task.FromResult((short)result);
        }

        public Task<PaintJobStatus> GetJobStatusAsync(short jobCode, CancellationToken cancellationToken = default)
        {
            if (jobCode < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(jobCode), jobCode, "Job code must be zero or greater.");
            }

            var result = _paintMixerDevice.QueryJobState(jobCode);

            PaintJobStatus status;

            switch (result)
            {
                case -1:
                    status = PaintJobStatus.Unknown;
                    break;

                case 0:
                    status = PaintJobStatus.Running;
                    break;

                case 1:
                    status = PaintJobStatus.Completed;
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Unexpected paint mixer status code returned: {result}.");
            }

            return Task.FromResult(status);
        }

        public Task CancelJobAsync(short jobCode, CancellationToken cancellationToken = default)
        {
            if (jobCode < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(jobCode), jobCode, "Job code must be zero or greater.");
            }

            var result = _paintMixerDevice.CancelJob(jobCode);

            if (result < 0)
            {
                throw new InvalidOperationException($"The paint mixer failed to cancel job {jobCode}.");
            }

            return Task.CompletedTask;
        }
    }
}
