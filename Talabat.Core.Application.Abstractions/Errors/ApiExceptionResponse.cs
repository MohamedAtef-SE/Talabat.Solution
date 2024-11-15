namespace Talabat.Core.Application.Abstractions.Errors
{
    public class ApiExceptionResponse : APIErrorResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int statusCode, string? message = null,string? details = null):base(statusCode,message)
        {
            Details = details ?? "No details available...";
        }
    }
}
