using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Domain.Entities.Identity;
using Talabat.Shared.Exceptions;

namespace Talabat.Core.Application
{
    public static class UserManagerExtentions
    {
        public static async Task<ApplicationUser> GetUserWithAdressAsync(this UserManager<ApplicationUser> userManager,ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.Users.Include(U => U.Address)
                                        .FirstOrDefaultAsync(U => U.Email == email);
            if (user is not { })
                throw new BadRequestException("an occurred error during getting user with its current address");

            return user;

            
        }
    }
}
