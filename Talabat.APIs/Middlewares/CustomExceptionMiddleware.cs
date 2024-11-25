using System.Net;
using System.Text.Json;
using Talabat.Core.Application.Abstractions.Errors;

namespace Talabat.APIs.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public CustomExceptionMiddleware(RequestDelegate next,ILogger<CustomExceptionMiddleware> logger,IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
                {
                _logger.LogError(ex,ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var apiExceptionResponse = _env.IsDevelopment() ?
                                           new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                                           :
                                           new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var JsonResponse = JsonSerializer.Serialize(apiExceptionResponse, options);

                await context.Response.WriteAsync(JsonResponse);
            }
        }
    }
}
