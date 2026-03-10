using FastEndpoints;
using PaintMixer.Service.Interfaces;

namespace PaintMixer.Api.Endpoints
{
    public sealed class CancelPaintJobEndpoint(IPaintMixerService paintMixerService) : EndpointWithoutRequest
    {
        private readonly IPaintMixerService _paintMixerService = paintMixerService;

        public override void Configure()
        {
            Delete("/api/paint-jobs/{jobCode}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken cancellationToken)
        {
            var jobCode = Route<short>("jobCode");
            await _paintMixerService.CancelJobAsync(jobCode, cancellationToken);
            await SendAsync(cancellationToken);
        }
    }
}
