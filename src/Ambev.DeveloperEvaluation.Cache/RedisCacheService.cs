using Ambev.DeveloperEvaluation.Common.Cache;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Cache;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _cache;
    private readonly TimeSpan _defaultExpiration;

    public RedisCacheService(IConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration), "A configuração do Redis não pode ser nula.");
        }

        var connectionString = configuration["Redis:ConnectionString"]
            ?? throw new ArgumentNullException(nameof(configuration), "A configuração do Redis não pode ser nula.");

        var expirationString = configuration["Redis:CacheExpirationInMinutes"];

        if (!int.TryParse(expirationString, out int expirationMinutes) || expirationMinutes <= 0)
        {
            expirationMinutes = 10;
        }

        var redis = ConnectionMultiplexer.Connect(connectionString);
        _cache = redis.GetDatabase();
        _defaultExpiration = TimeSpan.FromMinutes(expirationMinutes);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var data = await _cache.StringGetAsync(key);
        return data.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(data!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var jsonData = JsonSerializer.Serialize(value);
        await _cache.StringSetAsync(key, jsonData, expiration ?? _defaultExpiration);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.KeyDeleteAsync(key);
    }
}
