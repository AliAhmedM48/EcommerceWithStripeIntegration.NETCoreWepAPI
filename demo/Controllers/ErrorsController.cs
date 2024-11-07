using demo.Errors;
using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers;
[Route("error/{code}")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{

    public IActionResult Error(int code)
    {
        return NotFound(new ApiErrorResponse(StatusCodes.Status500InternalServerError, $"[{code}] is not found endpoint!"));
    }
}
