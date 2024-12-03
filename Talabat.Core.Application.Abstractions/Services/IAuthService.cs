using System.Security.Claims;
using Talabat.Shared.DTOModels._Common;
using Talabat.Shared.DTOModels.Auth;

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
