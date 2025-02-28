using Ambev.DeveloperEvaluation.Common.Cache;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Cache;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _cache;
    private readonly TimeSpan _defaultExpiration;
    private readonly IConnectionMultiplexer _redis;

    public RedisCacheService(IConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration), "A configuração do Redis não pode ser nula.");
        }

        var redisConfig = configuration.GetSection("Redis");
        var host = redisConfig["Host"] ?? "localhost";
        var port = redisConfig["Port"] ?? "6379";
        var password = redisConfig["Password"];

        var connectionString = password != null
            ? $"{host}:{port},password={password},abortConnect=false,connectRetry=5,connectTimeout=5000"
            : $"{host}:{port},abortConnect=false,connectRetry=5,connectTimeout=5000";

        if (!int.TryParse(redisConfig["CacheExpirationInMinutes"], out int expirationMinutes) || expirationMinutes <= 0)
        {
            expirationMinutes = 10;
        }

        _defaultExpiration = TimeSpan.FromMinutes(expirationMinutes);

        try
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _cache = _redis.GetDatabase();
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao conectar ao Redis: {ex.Message}");
        }
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
