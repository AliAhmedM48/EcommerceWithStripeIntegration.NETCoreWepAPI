using demo.Errors;
using Microsoft.AspNetCore.Mvc;
using Repository.Data;

namespace demo.Controllers;
public class BuggyController : BaseApiController
{
    private readonly StoreDbContext context;

    public BuggyController(StoreDbContext context)
    {
        this.context = context;
    }

    [HttpGet("notfound")]
    public async Task<IActionResult> GetNotFoundRequestError()
    {
        var brand = await context.Brands.FindAsync(500);
        //if (brand is null) return NotFound();
        if (brand is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, $"Brand with id:{100} is not found."));
        return Ok(brand);
    }

    [HttpGet("badrequest")]
    public async Task<IActionResult> GetBadRequestError()
    {
        return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, message: ""));
    }

    [HttpGet("unauthorized")]
    public async Task<IActionResult> GetUnauthorizedError()
    {
        return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
    }

    // -------------------------------------------------------------------------------------------------------

    [HttpGet("servererror")]
    public async Task<IActionResult> GetServerError()
    {
        var brand = await context.Brands.FindAsync(500);
        var brandString = brand.ToString();
        return Ok(brand);
    }

    [HttpGet("badrequest/{id}")] // id=ahmed => validation error
    public async Task<IActionResult> GetBadRequestError(int id)
    {
        return Ok();
    }
}
