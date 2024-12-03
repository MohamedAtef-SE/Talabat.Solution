using Talabat.Shared.Errors;

namespace Talabat.Shared.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message):base(message)
        {

        }
    }
}
