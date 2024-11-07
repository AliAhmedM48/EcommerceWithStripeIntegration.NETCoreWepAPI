using Core.Entities.Carts;

namespace Core;
public interface ICartRepository
{
    Task<Cart?> GetCartAsync(string cartId);
    Task<Cart?> UpdateCartAsync(Cart cart);
    Task<bool> DeleteCartAsync(string cartId);
    //void UpdateCart(Cart cart);
    //void Delete(TEntity entity);
}
