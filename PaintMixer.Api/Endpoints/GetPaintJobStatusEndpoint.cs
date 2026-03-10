using FastEndpoints;
using PaintMixer.Api.Dtos;
using PaintMixer.Service.Interfaces;

namespace PaintMixer.Api.Endpoints
{
    public sealed class GetPaintJobStatusEndpoint(IPaintMixerService paintMixerService) : 
        EndpointWithoutRequest<GetPaintJobStatusResponse>
    {
        private readonly IPaintMixerService _paintMixerService = paintMixerService;

        public override void Configure()
        {
            Get("/api/paint-jobs/{jobCode}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken cancellationToken)
        {
            var jobCode = Route<short>("jobCode");
            var status = await _paintMixerService.GetJobStatusAsync(jobCode, cancellationToken);

            await SendAsync(new GetPaintJobStatusResponse
            {
                JobCode = jobCode,
                Status = status.ToString()
            }, cancellation: cancellationToken);
        }
    }
}
