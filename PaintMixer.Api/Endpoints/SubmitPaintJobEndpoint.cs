using PaintMixer.Api.Dtos;
using PaintMixer.Domain;
using PaintMixer.Service.Interfaces;
using FastEndpoints;

namespace PaintMixer.Api.Endpoints
{
    public sealed class SubmitPaintJobEndpoint(IPaintMixerService paintMixerService) : 
        Endpoint<SubmitPaintJobRequest, SubmitPaintJobResponse>
    {
        private readonly IPaintMixerService _paintMixerService = paintMixerService;

        public override void Configure()
        {
            Post("/api/paint-jobs");
            AllowAnonymous();
        }

        public override async Task HandleAsync(SubmitPaintJobRequest request, CancellationToken cancellationToken)
        {
            var mix = new PaintMix(
                (byte)request.Red,
                (byte)request.Blue,
                (byte)request.Yellow,
                (byte)request.White,
                (byte)request.Black,
                (byte)request.Green);

            var jobCode = await _paintMixerService.SubmitJobAsync(mix, cancellationToken);

            await SendAsync(new SubmitPaintJobResponse
            {
                JobCode = jobCode
            }, cancellation: cancellationToken);
        }
    }
}
