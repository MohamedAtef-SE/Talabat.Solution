using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Core.Application.Services.Auth;
using Talabat.Core.Domain.Entities.Identity;
using Talabat.Shared.DTOModels.Auth;

namespace Route.Talabat.Dashboard.Controllers
{
    public class AdminController(UserManager<ApplicationUser> _userManager,IOptions<JwtSettings> _jwtSettings, ILogger<AdminController> _logger, SignInManager<ApplicationUser> _signInManager, IAuthService _authService) : Controller
    {
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index","Product");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  // This validates the anti-forgery token
        public async Task<IActionResult> Login(SignInDTO signInDTO)
        {
            try
            {
                var userDTO = await _authService.Login(signInDTO);

                // Store the token in a secure cookie
                CookieOptions cookieOptions = new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = false, // make it true in production environment.
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.DurationInMinutes)
                };

                Response.Cookies.Append("jwt_token", userDTO.Token,cookieOptions);

                var user = await _userManager.FindByEmailAsync(userDTO.Email);

                if (!await _userManager.IsInRoleAsync(user!, "Admin"))
                {
                    ModelState.AddModelError(string.Empty, "Admins Only");
                    return View();
                }
                
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Login));
            }
        }

        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies["jwt_token"] != null)
            {
                Response.Cookies.Delete("jwt_token");
            }
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }


    }
}
