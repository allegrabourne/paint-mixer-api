using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PaintMixer.Api.Diagnostics;
using PaintMixer.Api.Middleware;
using System.Text.Json;

namespace PaintMixer.Test
{

    public sealed class ObservabilityTests
    {
        [Fact]
        public async Task GivenAnArgumentException_WhenInvokeAsyncIsInvoked_ThenReturnsBadRequestErrorResponse()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            context.TraceIdentifier = "test-correlation-id";
            context.Request.Headers.AcceptLanguage = "fr-FR,fr;q=0.9";

            RequestDelegate next = _ => throw new ArgumentException("Invalid paint mix.");

            var loggerMock = new Mock<ILogger<Observability>>();
            var diagnosticMessageProviderMock = new Mock<IDiagnosticMessageProvider>();

            diagnosticMessageProviderMock
                .Setup(x => x.GetMessage("invalid_paint_mix", "fr"))
                .Returns("Le mélange de peinture est invalide. La quantité totale de colorant ne doit pas dépasser 100.");

            var sut = new Observability(next, loggerMock.Object);

            await sut.InvokeAsync(context, diagnosticMessageProviderMock.Object);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<ErrorResponse>(
                responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
            Assert.Equal("Le mélange de peinture est invalide. La quantité totale de colorant ne doit pas dépasser 100.", response!.Message);
        }

        [Fact]
        public async Task GivenAnUnhandledException_WhenInvokeAsyncCalled_ThenReturnsInternalServerErrorResponse()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            context.TraceIdentifier = "unexpected-error-id";

            RequestDelegate next = _ => throw new Exception("Boom");

            var loggerMock = new Mock<ILogger<Observability>>();
            var diagnosticMessageProviderMock = new Mock<IDiagnosticMessageProvider>();

            diagnosticMessageProviderMock
                .Setup(x => x.GetMessage("unexpected_error", It.IsAny<string?>()))
                .Returns("An unexpected error occurred while processing the request.");

            var sut = new Observability(next, loggerMock.Object);

            await sut.InvokeAsync(context, diagnosticMessageProviderMock.Object);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<ErrorResponse>(
                responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
            Assert.Equal("An unexpected error occurred while processing the request.", response.Message);
        }
    }
}