
namespace Talabat.Core.Application.Abstractions.Errors
{
    public class APIErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public APIErrorResponse(int statusCode,string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request 😥",
                401 => "UnAuthorized Request 😉",
                404 => "No Found 🤔",
                500 => "Internal Server Error 😬",
                _ => null
            };
        }
    }
}
