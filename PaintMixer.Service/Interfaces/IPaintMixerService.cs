using PaintMixer.Domain;
using PaintMixer.Domain.Enums;

namespace PaintMixer.Service.Interfaces
{
    public interface IPaintMixerService
    {
        Task<short> SubmitJobAsync(PaintMix paintMix, CancellationToken cancellationToken = default);

        Task<PaintJobStatus> GetJobStatusAsync(short jobCode, CancellationToken cancellationToken = default);

        Task CancelJobAsync(short jobCode, CancellationToken cancellationToken = default);
    }
}
