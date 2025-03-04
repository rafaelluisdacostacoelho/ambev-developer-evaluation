using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.Common.Cache;

[AttributeUsage(AttributeTargets.Method)]
public class CacheAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _cacheKeyTemplate;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };
    public int DurationInMinutes { get; set; } = 60;

    public CacheAttribute(string cacheKeyTemplate)
    {
        _cacheKeyTemplate = cacheKeyTemplate;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ICacheService? cacheService = context.HttpContext.RequestServices.GetService(typeof(ICacheService)) as ICacheService
            ?? throw new InvalidOperationException("Cache service not configured properly.");

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
            var serializedValue = JsonSerializer.Serialize(objectResult.Value, _jsonSerializerOptions);

            await cacheService.SetAsync(cacheKey, serializedValue, TimeSpan.FromMinutes(DurationInMinutes));
        }
    }

    private static string GenerateCacheKey(string template, ActionExecutingContext context)
    {
        var key = template;
        foreach (var parameter in context.ActionArguments)
        {
            key = Regex.Replace(key, $@"\{{{parameter.Key}\}}", parameter.Value?.ToString() ?? string.Empty);
        }
        return key;
    }
}
