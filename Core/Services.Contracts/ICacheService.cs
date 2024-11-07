namespace Core.Services.Contracts;
public interface ICacheService
{
    Task SetCacheKeyAsync(string key, object response, TimeSpan expireTime);
    Task<string?> GetCacheKeyAsync(string key);
}
