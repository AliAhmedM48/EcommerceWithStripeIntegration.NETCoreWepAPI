using Core;
using Core.Entities.Carts;
using StackExchange.Redis;
using System.Text.Json;

namespace Repository;
public class CartRepository : ICartRepository
{
    private readonly IDatabase database;

    public CartRepository(IConnectionMultiplexer redis)
    {
        database = redis.GetDatabase();
    }
    public async Task<bool> DeleteCartAsync(string cartId)
    {
        return await database.KeyDeleteAsync(cartId);
    }

    public async Task<Cart?> GetCartAsync(string cartId)
    {
        var cart = await database.StringGetAsync(cartId);

        return cart.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Cart>(cart);
    }

    public async Task<Cart?> UpdateCartAsync(Cart cart)
    {
        var createdOrUpdated = await database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(10));
        if (!createdOrUpdated) return null;

        return await GetCartAsync(cart.Id);
    }
}
