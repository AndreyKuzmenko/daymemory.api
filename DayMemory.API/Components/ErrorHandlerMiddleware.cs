using DayMemory.Core.Models.Exceptions;
using System.Net;
using System.Text.Json;

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
            catch (DuplicateItemException e)
            {
                logger.LogError(e, e.Message);
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDto($"Item with such an id='{e.ResourceId}' already exists!", 100)));
            }
            catch (ResourceNotFoundException e)
            {
                logger.LogError(e, e.Message);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDto("Resource not found")));
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDto("We're experiencing some problems. Try again later!")));
            }
        }
    }
}
