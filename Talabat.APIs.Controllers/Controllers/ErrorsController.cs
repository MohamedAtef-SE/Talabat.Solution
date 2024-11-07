using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.APIs.Controllers.Errors;

namespace Talabat.APIs.Controllers.Controllers
{
    [ApiController]
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Errors()
        {
            return NotFound(new APIErrorResponse(404));
        }
    }
}
