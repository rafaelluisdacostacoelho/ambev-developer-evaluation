using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.Common.Cache;

[AttributeUsage(AttributeTargets.Method)]
public class InvalidateCacheAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _cacheKeyTemplate;

    public InvalidateCacheAttribute(string cacheKeyTemplate)
    {
        _cacheKeyTemplate = cacheKeyTemplate;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<InvalidateCacheAttribute>)) as ILogger<InvalidateCacheAttribute>;

        if (context.HttpContext.RequestServices.GetService(typeof(ICacheService)) is not ICacheService cacheService)
        {
            logger?.LogError("Cache service not configured properly.");
            throw new InvalidOperationException("Cache service not configured properly.");
        }

        var cacheKey = GenerateCacheKey(_cacheKeyTemplate, context);

        await next();

        if (!string.IsNullOrEmpty(cacheKey))
        {
            bool removed = await cacheService.RemoveAsync(cacheKey);
            if (!removed)
            {
                logger?.LogWarning("Failed to remove cache for key: {CacheKey}. The key might not exist.", cacheKey);
            }
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