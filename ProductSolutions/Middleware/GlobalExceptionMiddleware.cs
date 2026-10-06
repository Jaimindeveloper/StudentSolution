using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ProductSolutions.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception while processing request {Method} {Path}", context.Request.Method, context.Request.Path);

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the global exception middleware will not be able to write the response.");
                    throw; // let the default behavior occur
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var accept = context.Request.Headers["Accept"].ToString();
            // If the client expects JSON (API), return a JSON ProblemDetails
            if (!string.IsNullOrEmpty(accept) && accept.Contains("application/json"))
            {
                var problem = new
                {
                    title = "An unexpected error occurred.",
                    status = (int)HttpStatusCode.InternalServerError,
                    detail = exception.Message
                };
                var json = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsync(json);
            }

            // For browser requests, redirect to the friendly error page
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.Redirect("/Home/Error");
            return Task.CompletedTask;
        }
    }
}
