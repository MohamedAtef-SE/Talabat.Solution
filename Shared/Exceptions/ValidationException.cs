namespace Talabat.Shared.Exceptions
{
    public class ValidationException : BadRequestException
    {
        public IEnumerable<string>? Errors { get; set; }
        public ValidationException(string message = "Bad Request") : base(message)
        {
        }
    }
}
