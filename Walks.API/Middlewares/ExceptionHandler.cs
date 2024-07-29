using System.Net;

namespace Walks.API.Middlewares
{
    public class ExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandler(ILogger<ExceptionHandler> logger, RequestDelegate next)
        {
            this._logger = logger;
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

                // --- Save on Log
                _logger.LogError(ex, $"ID {errorId} | ERR: ${ex.Message}");

                // --- Custom response error
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id=errorId,
                    Message="Something went wrong!"
                };

                await httpContext.Response.WriteAsJsonAsync(error);

            }
        }
    }
}
