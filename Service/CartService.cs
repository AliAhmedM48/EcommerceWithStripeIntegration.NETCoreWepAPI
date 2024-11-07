using AutoMapper;
using Core;
using Core.Dtos.Carts;
using Core.Entities.Carts;
using Core.Services.Contracts.Carts;

namespace Service;
public class CartService : ICartService
{
    private readonly ICartRepository cartRepository;
    private readonly IMapper mapper;

    public CartService(ICartRepository cartRepository, IMapper mapper)
    {
        this.cartRepository = cartRepository;
        this.mapper = mapper;
    }
    public async Task<bool> DeleteCartAsync(string cartId)
    {
        return await cartRepository.DeleteCartAsync(cartId);
    }

    public async Task<CartDto?> GetCartAsync(string cartId)
    {
        var cart = await cartRepository.GetCartAsync(cartId);
        if (cart is null) cart = new Cart() { Id = cartId };
        return mapper.Map<CartDto>(cart);
    }

    public async Task<CartDto?> UpdateCartAsync(CartDto cartDto)
    {
        var cart = await cartRepository.UpdateCartAsync(mapper.Map<Cart>(cartDto));

        return mapper.Map<CartDto>(cart);

    }
}
