using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.Common.Cache;

[AttributeUsage(AttributeTargets.Method)]
public class PaginatedCacheAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _cacheKeyTemplate;
    public int DurationInMinutes { get; set; } = 60;

    public PaginatedCacheAttribute(string cacheKeyTemplate)
    {
        _cacheKeyTemplate = cacheKeyTemplate;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheService = context.HttpContext.RequestServices.GetService(typeof(ICacheService)) as ICacheService;
        if (cacheService == null) throw new InvalidOperationException("Cache service not configured properly.");

        var cacheKey = GenerateCacheKey(_cacheKeyTemplate, context);

        var cachedValue = await cacheService.GetAsync<string>(cacheKey);
        if (!string.IsNullOrEmpty(cachedValue))
        {
            var contentResult = new ContentResult
            {
                Content = cachedValue,
                ContentType = "application/json",
                StatusCode = 200
            };
            context.Result = contentResult;
            return;
        }

        var executedContext = await next();

        if (executedContext.Result is ObjectResult objectResult && objectResult.StatusCode == 200)
        {
            var serializedValue = JsonSerializer.Serialize(objectResult.Value, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            await cacheService.SetAsync(cacheKey, serializedValue, TimeSpan.FromMinutes(DurationInMinutes));
        }
    }

    private static string GenerateCacheKey(string template, ActionExecutingContext context)
    {
        var key = template;

        foreach (var parameter in context.ActionArguments)
        {
            if (parameter.Value == null)
            {
                key = key.Replace($"{{{parameter.Key}}}", "null");
                continue;
            }

            if (IsSimpleType(parameter.Value.GetType()))
            {
                key = key.Replace($"{{{parameter.Key}}}", parameter.Value.ToString() ?? "null");
            }
            else
            {
                // Gera uma representação detalhada do objeto complexo na chave do cache
                var complexKey = GenerateComplexObjectKey(parameter.Value);
                key = key.Replace($"{{{parameter.Key}}}", complexKey);
            }
        }

        // Remove placeholders não preenchidos para evitar chaves malformadas
        key = Regex.Replace(key, @"\{.*?\}", "null");

        return key;
    }

    private static bool IsSimpleType(Type type)
    {
        return type.IsPrimitive || type.IsEnum || type.Equals(typeof(string)) || type.Equals(typeof(decimal));
    }

    private static string GenerateComplexObjectKey(object complexObject)
    {
        var properties = complexObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var keyValuePairs = properties
            .Where(p => p.CanRead && p.GetValue(complexObject) != null)
            .Select(p => $"{p.Name}={p.GetValue(complexObject)?.ToString()}")
            .ToList();

        return string.Join("_", keyValuePairs);
    }
}
