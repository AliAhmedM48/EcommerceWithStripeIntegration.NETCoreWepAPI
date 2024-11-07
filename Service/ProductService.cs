using AutoMapper;
using Core;
using Core.Dtos.Products;
using Core.Entities.Products;
using Core.Helper;
using Core.Services.Contracts.Products;
using Core.Specifications.Products;

namespace Service;
public class ProductService : IProductService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    //IRepository<Product, int> productRepository,
    //IRepository<ProductBrand, int> brandRepository,
    //IRepository<ProductType, int> typeRepository)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpecParams)
    {
        //return await productRepository.GetAllAsync();
        //return await unitOfWork.productRepository.GetAllAsync();
        //return await unitOfWork.GetRepository<Product, int>().GetAllAsync();
        //return mapper.Map<IEnumerable<ProductDto>>(await unitOfWork.GetRepository<Product, int>().GetAllAsync());

        var spec = new ProductSpecifications(productSpecParams);
        var products = mapper.Map<IEnumerable<ProductDto>>(await unitOfWork.GetRepository<Product, int>().GetAllWithSpecAsync(spec));

        var countSpec = new ProductCountSpecifications(productSpecParams);
        var count = await unitOfWork.GetRepository<Product, int>().GetCountWithSpecAsync(countSpec);

        return new PaginationResponse<ProductDto>(productSpecParams.pageSize, productSpecParams.pageIndex, count, products);
    }

    // -------------------------------------------
    public async Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync()
    {
        //return await brandRepository.GetAllAsync();
        //return await unitOfWork.productBrandRepository.GetAllAsync();
        //return await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
        return mapper.Map<IEnumerable<TypeBrandDto>>(await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync());
    }



    public async Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync()
    {
        //return await typeRepository.GetAllAsync();
        //return await unitOfWork.productTypeRepository.GetAllAsync();
        //return await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
        return mapper.Map<IEnumerable<TypeBrandDto>>(await unitOfWork.GetRepository<ProductType, int>().GetAllAsync());

    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        //return await productRepository.GetAsync(id);
        //return await unitOfWork.productRepository.GetAsync(id);
        //return await unitOfWork.GetRepository<Product, int>().GetAsync(id);
        //return mapper.Map<ProductDto>(await unitOfWork.GetRepository<Product, int>().GetAsync(id));

        var spec = new ProductSpecifications(id);
        return mapper.Map<ProductDto>(await unitOfWork.GetRepository<Product, int>().GetWithSpecAsync(spec));
    }
}
