namespace PaintMixer.Api.Diagnostics
{
    /// <summary>
    /// utility class that maps exceptions to appropriate HTTP status codes and error codes for API responses.
    /// Would be individualised and expanded in a real application to cover specific exception types 
    /// and scenarios relevant to the domain.
    /// </summary>
    public static class ExceptionMapper
    {
        public static (int StatusCode, string Code) Map(Exception exception)
        {
            return exception switch
            {
                ArgumentNullException => (StatusCodes.Status400BadRequest, "invalid_request"),
                ArgumentOutOfRangeException => (StatusCodes.Status400BadRequest, "invalid_request"),
                ArgumentException => (StatusCodes.Status400BadRequest, "invalid_paint_mix"),

                InvalidOperationException ex when ex.Message.Contains("submit", StringComparison.OrdinalIgnoreCase)
                    => (StatusCodes.Status409Conflict, "job_submission_failed"),

                InvalidOperationException ex when ex.Message.Contains("status", StringComparison.OrdinalIgnoreCase)
                    => (StatusCodes.Status500InternalServerError, "job_status_failed"),

                InvalidOperationException ex when ex.Message.Contains("cancel", StringComparison.OrdinalIgnoreCase)
                    => (StatusCodes.Status409Conflict, "job_cancellation_failed"),

                KeyNotFoundException => (StatusCodes.Status404NotFound, "job_not_found"),

                _ => (StatusCodes.Status500InternalServerError, "unexpected_error")
            };
        }
    }
}
