using Core.Dtos.Products;
using Core.Helper;
using Core.Services.Contracts.Products;
using Core.Specifications.Products;
using demo.Attributes;
using demo.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers;
public class ProductsController : BaseApiController
{
    private readonly IProductService productService;

    public ProductsController(IProductService productService)
    {
        this.productService = productService;
    }

    [HttpGet]
    [Authorize]
    [Cached(100)]
    [ProducesResponseType(typeof(PaginationResponse<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProducts([FromQuery] string? search, string? sort, int? brandId, int? typeId, int pageSize = 1, int pageIndex = 1)
    {
        await Task.Delay(10000);
        var productSpecParams = new ProductSpecParams(sort, brandId, typeId, pageSize, pageIndex, search);
        var products = await productService.GetAllProductsAsync(productSpecParams);

        return Ok(products);
    }


    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int? id)
    {
        if (id is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid Id"));
        var product = await productService.GetProductByIdAsync(id.Value);
        if (product is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, $"The Product with id: {id} is not found"));

        return Ok(product);
    }

    [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
    [HttpGet("brands")]
    public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllBrands()
    {
        var brands = await productService.GetAllBrandsAsync();

        return Ok(brands);
    }


    [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
    [HttpGet("types")]
    public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllTypes()
    {
        var types = await productService.GetAllTypesAsync();

        return Ok(types);
    }
}
