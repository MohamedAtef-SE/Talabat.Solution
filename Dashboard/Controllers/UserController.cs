using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Domain.Entities.Identity;

namespace Route.Talabat.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController(UserManager<ApplicationUser> _userManager,RoleManager<IdentityRole> _roleManager) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select( u => new UserViewModel()
            {
                Id = u.Id,
                DisplayName = u.DisplayName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                UserName = u.UserName,
                Roles =  _userManager.GetRolesAsync(u).Result,

            }).ToListAsync();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var allRoles = await _roleManager.Roles.ToListAsync();
            var viewModel = new UserRoleViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = allRoles.Select(
                    R => new EditRoleViewModel()
                    {
                        Id = R.Id,
                        Name = R.Name,
                        isSelected = _userManager.IsInRoleAsync(user,R.Name).Result
                    }).ToList(),
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  // This validates the anti-forgery token
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if(userRoles.Any(r => r == role.Name) && !role.isSelected)
                    await _userManager.RemoveFromRoleAsync(user,role.Name);
                if (!userRoles.Any(r => r == role.Name) && role.isSelected)
                    await _userManager.AddToRoleAsync(user, role.Name);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
