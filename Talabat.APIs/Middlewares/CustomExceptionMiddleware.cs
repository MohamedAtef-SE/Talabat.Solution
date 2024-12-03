using System.Net;
using System.Text.Json;
using Talabat.Shared.Errors;
using Talabat.Shared.Exceptions;

namespace Talabat.APIs.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger, IHostEnvironment env)
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
                if (_env.IsDevelopment())
                {
                    _logger.LogError(ex, ex.Message);
                }
                else
                {
                    // Production Mode
                    // log exception Details in Database || JSON
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            APIErrorResponse apiErrorResponse;
            context.Response.ContentType = "application/json";
            var jsonSerializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            switch (ex)
            {
                case NotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    apiErrorResponse = new APIErrorResponse(context.Response.StatusCode, ex.Message);

                    var SerializedNotFound = JsonSerializer.Serialize(apiErrorResponse, jsonSerializerOptions);
                    await context.Response.WriteAsync(SerializedNotFound);
                    break;

                case ValidationException validation:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    apiErrorResponse = new ApiValidationResponse(context.Response.StatusCode, ex.Message) { Errors = validation.Errors };

                    var SerializedValidationResponse = JsonSerializer.Serialize(apiErrorResponse, jsonSerializerOptions);
                    await context.Response.WriteAsync(SerializedValidationResponse);
                    break;

                case BadRequestException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    apiErrorResponse = new APIErrorResponse(context.Response.StatusCode, ex.Message);

                    var SerializedBadRequest = JsonSerializer.Serialize(apiErrorResponse, jsonSerializerOptions);
                    await context.Response.WriteAsync(SerializedBadRequest);
                    break;

                case UnauthorizedException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    apiErrorResponse = new APIErrorResponse(context.Response.StatusCode, ex.Message);

                    var SerializedUnauthorizedException = JsonSerializer.Serialize(apiErrorResponse, jsonSerializerOptions);
                    await context.Response.WriteAsync(SerializedUnauthorizedException);
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    apiErrorResponse = _env.IsDevelopment() ?
                                               new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                                               :
                                               new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

                    var JsonResponse = JsonSerializer.Serialize(apiErrorResponse, jsonSerializerOptions);
                    await context.Response.WriteAsync(JsonResponse);
                    break;
            }
        }
    }
}
