namespace Core.Dtos.Products;
public class ProductDto
{
    public Guid GUID { get; set; } = Guid.NewGuid();
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PictureUrl { get; set; }
    public decimal Price { get; set; }

    // Navigational Properties
    //public int? BrandId { get; set; }        // FK
    public string BrandName { get; set; }

    //public int? TypeId { get; set; }        // FK
    public string TypeName { get; set; }
    public DateTime CreateAt { get; set; }
}
