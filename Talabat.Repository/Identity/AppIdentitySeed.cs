using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
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
                var UsersAsJson = File.ReadAllText("../Talabat.Repository/Identity/Seeds/users.json");

                var users = JsonSerializer.Deserialize<List<ApplicationUser>>(UsersAsJson);

                if (users is null) return;

                foreach (var user in users)
                {
                    await _userManager.CreateAsync(user, "P@ssw0rd");
                }
            }
        }
    }
}
