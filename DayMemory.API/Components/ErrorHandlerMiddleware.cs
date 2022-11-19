using System.Net;

namespace DayMemory.API.Components
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ErrorHandlerMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                logger.LogError(error, "Unhandled error");

                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.ContentType = "text/html";
                await response.WriteAsync("We're experiencing some problems. Try again later!");
            }
        }

    }
}
