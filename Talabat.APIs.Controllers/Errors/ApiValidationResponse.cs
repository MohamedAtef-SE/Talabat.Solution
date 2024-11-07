namespace Talabat.APIs.Controllers.Errors
{
    public class ApiValidationResponse : APIErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationResponse(int statusCode,string? message):base(statusCode,message)
        {
            Errors = new List<string>();
        }
    }
}
