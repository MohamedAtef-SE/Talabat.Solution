using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Talabat.Core.Domain.Entities.Identity;
using Talabat.Shared.Exceptions;

namespace Talabat.Infrastructure.Persistence.Identity
{
    public class AppIdentitySeed
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AppIdentitySeed(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task UploadUsersAsync()
        {
            if (!_userManager.Users.Any())
            {
                var filePath = Path.Combine("../Talabat.Repository", "Identity", "Seeds", "users.json");

                if (File.Exists(filePath))
                {
                    var SerializedUsers = File.ReadAllText(filePath);

                    var users = JsonSerializer.Deserialize<List<ApplicationUser>>(SerializedUsers);

                    if (users is null) return;

                    foreach (var user in users)
                    {
                        await _userManager.CreateAsync(user, "P@ssw0rd");
                    }
                }
                else
                {
                    throw new NotFoundException($"Incorrect path found < {Path.GetFullPath(filePath)} >");
                }
                
            }
        }
    }
}
