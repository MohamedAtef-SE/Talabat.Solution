using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Route.Talabat.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController(RoleManager<IdentityRole> _roleManager) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // get all the roles
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]  // This validates the anti-forgery token
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            if(!ModelState.IsValid) return RedirectToAction(nameof(Index));

            var isExist = await _roleManager.RoleExistsAsync(model.Name.Trim());
            if (!isExist)
            {
                var newIdentityRole = new IdentityRole()
                {
                    Name = model.Name.Trim(),
                };

                await _roleManager.CreateAsync(newIdentityRole);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Name","Role is already exist");
                return View(nameof(Index), await _roleManager.Roles.ToListAsync());
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
            {
                ModelState.AddModelError(string.Empty, "Can't find the role....");
                return RedirectToAction(nameof(Index));
            }

            var RoleVM = new EditRoleViewModel()
            {
                Name = role.Name,
            };
            return View(RoleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  // This validates the anti-forgery token
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role is { })
            {
                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Something went wrong can't update this role at this moment");
            return View(model);

            


        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            
            if (role is { })
            {
                await _roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));
            };
            ModelState.AddModelError(string.Empty, "Can't Delete this role at this moment");
            return View(nameof(Index), await _roleManager.Roles.ToListAsync());
            //return RedirectToAction(nameof(Index));
            //throw new BadRequestException("Bad Request, Can't Delete this role at this moment");

        }
    }
}
