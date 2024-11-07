using AutoMapper;
using Core.Dtos.Orders;
using Core.Entities.Order;
using Microsoft.Extensions.Configuration;

namespace Core.Mapping.Orders;
public class OrderProfile : Profile
{
    public OrderProfile(IConfiguration configuration)
    {
        CreateMap<Order, OrderReturnedDto>()
            .ForMember(o => o.DeliveryMethodName,
            o => o.MapFrom(p => p.DeliveryMethod.ShortName))
            .ForMember(p => p.DeliveryMethodCost,
            o => o.MapFrom(p => p.DeliveryMethod.Cost))
            .ForMember(m => m.Total, p => p.MapFrom(p => p.geetTotal()));

        CreateMap<ShippingAddress, ShippingAddressDto>().ReverseMap();

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(o => o.ProductId,
            o => o.MapFrom(p => p.Product.ProductId))
            .ForMember(o => o.ProductName,
            o => o.MapFrom(p => p.Product.ProductName))
            .ForMember(o => o.PictureUrl,
            o => o.MapFrom(p => $"{configuration["BASEURL"]}/{p.Product.PictureUrl}"));
    }
}
