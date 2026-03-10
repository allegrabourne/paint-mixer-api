using PaintMixer.Api.Diagnostics;

namespace PaintMixer.Api.Middleware
{
    /// <summary>
    /// Middleware that provides observability for the API by catching unhandled exceptions, 
    /// logging them with appropriate diagnostic codes and correlation IDs, and returning standardised
    /// error responses to clients. This middleware ensures that all exceptions are consistently handled 
    /// and that clients receive meaningful error information while also enabling effective 
    /// monitoring and troubleshooting through structured logging.
    /// </summary>
    public sealed class Observability
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<Observability> _logger;

        public Observability(
            RequestDelegate next,
            ILogger<Observability> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IDiagnosticMessageProvider diagnosticMessageProvider)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var (statusCode, code) = ExceptionMapper.Map(exception);
                var correlationId = context.TraceIdentifier;
                var culture = GetPreferredLanguage(context);

                _logger.LogError(
                    exception,
                    "Request failed with diagnostic code {DiagnosticCode}. CorrelationId: {CorrelationId}",
                    code,
                    correlationId);

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var response = new ErrorResponse
                {
                    Code = code,
                    Message = diagnosticMessageProvider.GetMessage(code, culture),
                    CorrelationId = correlationId
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }

        private static string GetPreferredLanguage(HttpContext context)
        {
            var acceptLanguage = context.Request.Headers.AcceptLanguage.ToString();

            if (string.IsNullOrWhiteSpace(acceptLanguage))
            {
                return "en";
            }

            return acceptLanguage
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(';')[0].Trim())
                .FirstOrDefault()?
                .Split('-')[0]
                ?? "en";
        }
    }
}