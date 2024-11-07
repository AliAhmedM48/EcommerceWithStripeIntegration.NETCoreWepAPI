using Core.Dtos.Products;
using Core.Helper;
using Core.Specifications.Products;

namespace Core.Services.Contracts.Products;
public interface IProductService
{
    Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpecParams);
    Task<ProductDto> GetProductByIdAsync(int id);

    // ----------------------------------------------------------

    Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync();
    Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync();

    // ----------------------------------------------------------

    //Task AddAsync(Product entity);
    //void Update(Product entity);
    //void Delete(Product entity);
}
