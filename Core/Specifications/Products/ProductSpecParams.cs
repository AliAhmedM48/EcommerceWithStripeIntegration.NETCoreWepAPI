namespace Core.Specifications.Products;
public class ProductSpecParams
{
    public string? sort;
    public int? brandId;
    public int? typeId;
    public int pageSize;
    public int pageIndex;
    public string? search;

    public ProductSpecParams(string? sort, int? brandId, int? typeId, int pageSize, int pageIndex, string? search)
    {
        this.sort = sort;
        this.brandId = brandId;
        this.typeId = typeId;
        this.pageSize = pageSize;
        this.pageIndex = pageIndex;
        this.search = search?.ToLower();
    }
}
