using Core.Entities.Products;

namespace Core.Specifications.Products;

public class ProductSpecifications : BaseSpecifications<Product, int>
{
    public ProductSpecifications(int id) : base(p => p.Id == id)
    {
        ApplyIncludes();

    }

    public ProductSpecifications(ProductSpecParams productSpecParams)
        : base(p =>
        (string.IsNullOrWhiteSpace(productSpecParams.search) || p.Name.ToLower().Contains(productSpecParams.search))
        &&
        (!productSpecParams.brandId.HasValue || p.BrandId == productSpecParams.brandId)
        &&
        (!productSpecParams.typeId.HasValue || p.TypeId == productSpecParams.typeId)
        )
    {
        // name - priceAsc - priceDesc

        if (!string.IsNullOrWhiteSpace(productSpecParams.sort))
        {
            switch (productSpecParams.sort.ToLower())
            {
                case "priceasc":
                    AddOrderBy(p => p.Price);
                    break;

                case "pricedesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
        else
        {
            AddOrderBy(p => p.Id);
        }
        ApplyIncludes();

        ApplyPagination(productSpecParams.pageSize * (productSpecParams.pageIndex - 1), productSpecParams.pageSize);
    }

    private void ApplyIncludes()
    {
        Includes.Add(p => p.Brand);
        Includes.Add(p => p.Type);
    }

}