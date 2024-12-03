using System.Security.Claims;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Shared.Exceptions;

namespace Talabat.APIs.Services
{
    public class LoggedInUserService : ILoggedInUserService
    {
        public string? UserId { get; }
        public LoggedInUserService(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor is null)
                throw new BadRequestException("an occurred error during access HttpContextAccessor");
                
               UserId = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.PrimarySid);
        }
    }
}
