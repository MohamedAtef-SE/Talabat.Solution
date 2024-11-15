using Microsoft.AspNetCore.Identity;

namespace Talabat.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public required string DisplayName { get; set; }
        public virtual Address Address { get; set; } = null!;
    }
}
