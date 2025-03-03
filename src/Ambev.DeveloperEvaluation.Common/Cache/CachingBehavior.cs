using MediatR;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.Common.Cache;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ICacheService _cache;

    public CachingBehavior(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Identifica o tipo de método HTTP associado à request
        var httpMethod = GetHttpMethod(request);

        // Apenas continua o cacheamento para rotas GET
        if (httpMethod != "GET")
        {
            return await next();
        }

        string cacheKey = GenerateCacheKey(request);

        if (string.IsNullOrEmpty(cacheKey))
        {
            return await next();
        }

        var cachedResponse = await _cache.GetAsync<TResponse>(cacheKey);
        if (cachedResponse is not null)
        {
            return cachedResponse;
        }

        var response = await next();
        await _cache.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));

        return response;
    }

    private static string GetHttpMethod(TRequest request)
    {
        var typeName = request.GetType().Name.ToLower();

        // Identifica o método HTTP baseado no padrão de nomenclatura da request
        if (typeName.StartsWith("pagination")) return "UNKNOWN";
        if (typeName.EndsWith("query") || typeName.StartsWith("get") || typeName.StartsWith("list")) return "GET";
        if (typeName.StartsWith("create") || typeName.StartsWith("add") || typeName.StartsWith("post")) return "POST";
        if (typeName.StartsWith("update") || typeName.StartsWith("edit") || typeName.StartsWith("put")) return "PUT";
        if (typeName.StartsWith("patch")) return "PATCH";
        if (typeName.StartsWith("delete") || typeName.StartsWith("remove")) return "DELETE";

        return "UNKNOWN";
    }

    private static string GenerateCacheKey(TRequest request)
    {
        var actionName = request.GetType().Name.Replace("Query", "").Replace("Command", "");
        var entityName = actionName.Replace("Get", "");
        var properties = request.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var key = $"Cache:{entityName}:{actionName}";

        foreach (var property in properties)
        {
            var propertyName = property.Name;
            var value = property.GetValue(request)?.ToString() ?? "null";
            key += $":{propertyName}={value}";
        }

        // Substitui caracteres inválidos para o Redis
        return Regex.Replace(key, @"[^a-zA-Z0-9:_=]", "_");
    }
}
