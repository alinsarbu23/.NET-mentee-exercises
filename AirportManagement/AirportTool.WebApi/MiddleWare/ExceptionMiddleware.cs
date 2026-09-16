using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AirportTool.WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            var problem = new
            {
                title = "An unexpected error occurred.",
                detail = exception.Message
            };

            if (exception is ArgumentException)
            {
                code = HttpStatusCode.BadRequest;
            }
            else if (exception is KeyNotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }
            else if (exception is InvalidOperationException)
            {
                code = HttpStatusCode.Conflict;
            }

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)code;

            var json = JsonSerializer.Serialize(new
            {
                type = $"https://httpstatuses.org/{(int)code}",
                title = problem.title,
                detail = problem.detail,
                status = (int)code
            });

            return context.Response.WriteAsync(json);
        }
    }
}
