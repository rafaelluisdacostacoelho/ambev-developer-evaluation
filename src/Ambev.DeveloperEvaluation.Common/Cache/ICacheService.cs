namespace Ambev.DeveloperEvaluation.Common.Cache;

public interface ICacheService
{
    Task SetAsync<T>(string key, T value, TimeSpan? expiration);
    Task<T?> GetAsync<T>(string key);
}
