using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Shared.DTOModels._Common;
using Talabat.Shared.DTOModels.Auth;
using Talabat.Shared.Errors;

namespace Talabat.APIs.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost("Register")]
        [ProducesResponseType(typeof(UserDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIErrorResponse),statusCode: StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var result = await _serviceManager.AuthService.Register(registerDTO);

            return result is not null ? Ok(result) : BadRequest(new APIErrorResponse(400, "Issues found with registeration proccess.😬"));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> LoginAsync(SignInDTO signInDTO)
        {
            var result = await _serviceManager.AuthService.Login(signInDTO);

            return result is not null ? Ok(result) : BadRequest(new APIErrorResponse(400, "Issues found with login proccess.😬"));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
           var result =  await _serviceManager.AuthService.GetCurrentUser();
            return result is not null ? Ok(result) : BadRequest(new APIErrorResponse(400, "Issues found while getting current user.😬"));
        }

        [HttpGet("Address")]
        [Authorize]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            var result = await _serviceManager.AuthService.GetUserAddressAsync(User);
            return result is not null ? Ok(result) : BadRequest(new APIErrorResponse(400, "Issues found while getting user address.😬"));
        }

        [HttpPut("Address")]
        [Authorize]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO addressDTO)
        {
            var result = await _serviceManager.AuthService.UpdateAddress(User, addressDTO);
            return result is not null ? Ok(result) : BadRequest(new APIErrorResponse(400, "Issues found while updating user address.😬"));
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _serviceManager.AuthService.checkEmailExists(email);
        }

    }
}
