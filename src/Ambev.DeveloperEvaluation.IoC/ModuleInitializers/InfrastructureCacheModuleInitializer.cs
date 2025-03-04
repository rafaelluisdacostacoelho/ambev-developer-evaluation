using Ambev.DeveloperEvaluation.Cache;
using Ambev.DeveloperEvaluation.Common.Cache;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureCacheModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        var redisConfig = builder.Configuration.GetSection("Redis").Get<RedisConfiguration>()
            ?? throw new ArgumentNullException(nameof(builder), "Configuração do Redis não encontrada.");

        var connectionString = BuildConnectionString(redisConfig);

        // Health Check para o Redis
        builder.Services.AddHealthChecks()
                        .AddRedis(connectionString, "Redis Health", HealthStatus.Degraded);

        // Configurando o Redis como Singleton
        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            try
            {
                return ConnectionMultiplexer.Connect(connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao conectar ao Redis: {ex.Message}");
                throw;
            }
        });

        // Registro do serviço de cache
        builder.Services.AddSingleton<ICacheService, RedisCacheService>();
    }

    // Método para construir a string de conexão do Redis
    private static string BuildConnectionString(RedisConfiguration config)
    {
        return $"{config.Host}:{config.Port},password={config.Password},abortConnect=false,connectRetry=5,connectTimeout=10000";
    }
}

// Configuração tipada para o Redis
public class RedisConfiguration
{
    public string Host { get; set; } = "localhost";
    public string Port { get; set; } = "6379";
    public string Password { get; set; } = string.Empty;
    public int CacheExpirationInMinutes { get; set; } = 10;
}
