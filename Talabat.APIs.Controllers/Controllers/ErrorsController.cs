using Microsoft.AspNetCore.Mvc;
using Talabat.Shared.Errors;

namespace Talabat.APIs.Controllers.Controllers
{
    [ApiController]
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Errors(int Code)
        {
            if(Code is 404)
                return NotFound(new ApiExceptionResponse(Code));

            else if(Code is 401)
                return Unauthorized(new ApiExceptionResponse(Code));

            else return BadRequest(new ApiExceptionResponse(Code));
        }
    }
}
