using AutoMapper;
using Core.Dtos.Products;
using Core.Entities.Products;
using Microsoft.Extensions.Configuration;

namespace Core.Mapping.Products;
public class ProductProfile : Profile
{
    public ProductProfile(IConfiguration configuration)
    {
        CreateMap<Product, ProductDto>()
            .ForMember(d => d.BrandName,
            options => options.MapFrom(s => s.Brand.Name))
            .ForMember(d => d.TypeName,
            options => options.MapFrom(s => s.Type.Name))
            .ForMember(d => d.PictureUrl,
            options => options.MapFrom(s => $"{configuration["BASEURL"]}/{s.PictureUrl}"));

        CreateMap<ProductBrand, TypeBrandDto>();
        CreateMap<ProductType, TypeBrandDto>();
    }
}
