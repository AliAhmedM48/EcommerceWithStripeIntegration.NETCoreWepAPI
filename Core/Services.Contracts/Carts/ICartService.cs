using Core.Dtos.Carts;

namespace Core.Services.Contracts.Carts;
public interface ICartService
{
    public Task<CartDto?> GetCartAsync(string cartId);
    public Task<bool> DeleteCartAsync(string cartId);
    public Task<CartDto?> UpdateCartAsync(CartDto cart);
}
