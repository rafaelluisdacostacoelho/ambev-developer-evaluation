using Ambev.DeveloperEvaluation.Application.Cache;
using Ambev.DeveloperEvaluation.Cache;
using Ambev.DeveloperEvaluation.Common.Cache;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

class InfrastructureCacheModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        var redisConfig = builder.Configuration.GetSection("Redis");
        var host = redisConfig["Host"] ?? "localhost";
        var port = redisConfig["Port"] ?? "6379";
        var password = redisConfig["Password"];

        var connectionString = password != null
            ? $"{host}:{port},password={password},abortConnect=false,connectRetry=5,connectTimeout=5000"
            : $"{host}:{port},abortConnect=false,connectRetry=5,connectTimeout=5000";

        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(connectionString));

        builder.Services.AddSingleton<ICacheService, RedisCacheService>();

        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationBehavior<,>));
    }
}