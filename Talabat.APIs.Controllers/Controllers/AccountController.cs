using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Application.Abstractions.DTOModels;
using Talabat.Core.Application.Abstractions.DTOModels.Auth;
using Talabat.Core.Application.Abstractions.Errors;
using Talabat.Core.Application.Abstractions.Services;

namespace Talabat.APIs.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(UserDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIErrorResponse),statusCode: StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var userDTO = await _authService.Register(registerDTO);
            return userDTO is not null ? Ok(userDTO) : Conflict(new APIErrorResponse(409,"Email is already exist"));
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult<UserDTO>> SignIn(SignInDTO signInDTO)
        {
            var userDTO = await _authService.Login(signInDTO);
            
            return Ok(userDTO);
        }

        [HttpGet("GetUser")]
        [Authorize]
        public async Task<UserDTO> GetCurrentUser()
        {
           return await _authService.GetCurrentUser();
        }

        [HttpGet("UserAddress")]
        [Authorize]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            var addressDTO = await _authService.GetUserAddressAsync(User);
            return Ok(addressDTO);
        }


        [HttpPut("ChangeUserAddress")]
        [Authorize]
        public async Task<ActionResult<AddressDTO>> UpadateUserAddress(AddressDTO addressDTO)
        {
            return await _authService.UpdateAddress(User, addressDTO);
        }

    }
}
