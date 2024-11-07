using AutoMapper;
using Core.Dtos.Carts;
using Core.Entities.Carts;

namespace Core.Mapping.Carts;
public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<Cart, CartDto>().ReverseMap();
        CreateMap<CartItem, CartItemDto>().ReverseMap();
    }
}
