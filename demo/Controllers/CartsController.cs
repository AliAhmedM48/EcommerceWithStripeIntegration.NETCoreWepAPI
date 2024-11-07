using Core.Dtos.Carts;
using Core.Entities.Carts;
using Core.Services.Contracts.Carts;
using demo.Errors;
using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers;
public class CartsController : BaseApiController
{
    private readonly ICartService cartService;

    public CartsController(ICartService cartService)
    {
        this.cartService = cartService;
    }

    [ProducesResponseType(typeof(Cart), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [HttpGet("{id}")]
    public async Task<ActionResult<Cart>> GetCartById(string? id)
    {
        if (id == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid Id."));

        var cart = await cartService.GetCartAsync(id);
        //if (cart is null) cart = mapper.Map<CartDto>(new Cart() { Id = id });
        if (cart is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

        return Ok(cart);
    }


    [HttpPost]
    [ProducesResponseType(typeof(Cart), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Cart>> CreateOrUpdateCart(CartDto? model)
    {
        if (model is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid data"));
        var cart = await cartService.UpdateCartAsync(model);
        if (cart is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

        return Ok(cart);
    }

    [HttpDelete("{id}")]

    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCart(string? id)
    {
        if (id is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid Id."));
        var flag = await cartService.DeleteCartAsync(id);
        if (!flag) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
        return NoContent();
    }
}
