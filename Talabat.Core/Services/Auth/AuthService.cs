using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Talabat.Core.Application.Abstractions.DTOModels;
using Talabat.Core.Application.Abstractions.DTOModels.Auth;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Core.Domain.Entities.Identity;

namespace Talabat.Core.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;
        public AuthService(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IOptions<JwtSettings> jwtSettings, IHttpContextAccessor httpContextAccessor,
                           IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<UserDTO?> Register(RegisterDTO registerDTO)
        {

            var userFound = await _userManager.FindByEmailAsync(registerDTO.Email);

            if (userFound is { }) return null;

            var newUser = new ApplicationUser()
            {
                DisplayName = registerDTO.DisplayName,
                UserName = registerDTO.Email.Split('@')[0],
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber
            };
            var result = await _userManager.CreateAsync(newUser, registerDTO.Password);

            var userDTO = new UserDTO()
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                Token = await GenerateTokenAsync(newUser)
            };

            return userDTO;
        }
        public async Task<UserDTO> Login(SignInDTO signInDTO)
        {
            var user = await _userManager.FindByEmailAsync(signInDTO.Email);

            var userStatus = await _signInManager.CheckPasswordSignInAsync(user, signInDTO.Password, false);

            var userDTO = new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateTokenAsync(user)
            };
            return userDTO;
        }
        public async Task<UserDTO> GetCurrentUser()
        {
            var email = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            return new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateTokenAsync(user)
            };
        }
        public async Task<AddressDTO> UpdateAddress(ClaimsPrincipal User, AddressDTO addressDTO)
        {
            var user = await _userManager.GetUserWithAdressAsync(User);

            addressDTO.Id = user.Address?.Id ?? null;

            var mappedAddress = _mapper.Map<AddressDTO,Address>(addressDTO);

            user.Address = mappedAddress;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception("Address failed to updated.");

            return addressDTO;

        }
        public async Task<AddressDTO> GetUserAddressAsync(ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.GetUserWithAdressAsync(User);

            var MappedAddress = _mapper.Map<Address, AddressDTO>(user.Address);

            return MappedAddress;
        }
        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var privateClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.PrimarySid, user.Id),
                new Claim(ClaimTypes.Email, user.Email?? "N/A"),
                new Claim(ClaimTypes.GivenName, user.DisplayName),
                new Claim(ClaimTypes.NameIdentifier,user.UserName??"N/A"),
                new Claim(ClaimTypes.Role,userRoles.FirstOrDefault()??"N/A")

            };

            var SerurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((_jwtSettings.Key)));

            var Token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: new SigningCredentials(SerurityKey, SecurityAlgorithms.HmacSha256),
                claims: privateClaims
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
        public async Task<bool> checkEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
