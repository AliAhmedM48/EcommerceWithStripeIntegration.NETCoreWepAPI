using Core.Services.Contracts;
using StackExchange.Redis;
using System.Text.Json;

namespace Service;
public class CacheService : ICacheService
{
    private readonly IDatabase _database;
    public CacheService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }
    public async Task<string?> GetCacheKeyAsync(string key)
    {
        return await _database.StringGetAsync(key);
    }

    public async Task SetCacheKeyAsync(string key, object response, TimeSpan expireTime)
    {
        if (response is null) throw new ArgumentNullException();
        var serializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var responseJSON = JsonSerializer.Serialize(response, serializerOptions);
        await _database.StringSetAsync(key, responseJSON, expireTime);
    }
}
