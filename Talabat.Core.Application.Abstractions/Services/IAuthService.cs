using System.Security.Claims;
using Talabat.Core.Application.Abstractions.DTOModels;
using Talabat.Core.Application.Abstractions.DTOModels.Auth;

namespace Talabat.Core.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<UserDTO?> Register(RegisterDTO registerDTO);
        Task<UserDTO> Login(SignInDTO loginDTO);
        Task<UserDTO> GetCurrentUser();
        Task<AddressDTO> GetUserAddressAsync(ClaimsPrincipal User);
        Task<AddressDTO> UpdateAddress(ClaimsPrincipal User, AddressDTO addressDTO);
        Task<bool> checkEmailExists(string email);
    }
}
