using Ambev.DeveloperEvaluation.Common.Cache;
using MediatR;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Application.Cache;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ICacheService _cache;

    public CachingBehavior(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var cacheKey = $"Cache_{typeof(TRequest).Name}_{JsonSerializer.Serialize(request)}";
        var cachedResponse = await _cache.GetAsync<TResponse>(cacheKey);
        if (cachedResponse is not null)
        {
            return cachedResponse;
        }

        var response = await next();
        await _cache.SetAsync(cacheKey, response, TimeSpan.FromMinutes(10));

        return response;
    }
}
