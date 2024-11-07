using Core.Entities.Products;

namespace Core.Specifications.Products;

public class ProductCountSpecifications : BaseSpecifications<Product, int>
{
    public ProductCountSpecifications(ProductSpecParams productSpecParams)
        : base(p =>
                (string.IsNullOrWhiteSpace(productSpecParams.search) || p.Name.ToLower().Contains(productSpecParams.search))
        &&
            (!productSpecParams.brandId.HasValue || p.BrandId == productSpecParams.brandId)
            &&
            (!productSpecParams.typeId.HasValue || p.TypeId == productSpecParams.typeId)
        )
    {

    }
}