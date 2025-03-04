using Ambev.DeveloperEvaluation.Common.Cache;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Cache;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase _database;
    private readonly int _cacheExpirationInMinutes;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, IConfiguration configuration)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _database = _connectionMultiplexer.GetDatabase();
        _cacheExpirationInMinutes = int.Parse(configuration["Redis:CacheExpirationInMinutes"] ?? "10");
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return default;
        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration)
    {
        expiration ??= TimeSpan.FromMinutes(_cacheExpirationInMinutes);
        var serializedValue = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, serializedValue, expiration);
    }

    public async Task<bool> RemoveAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }
}
