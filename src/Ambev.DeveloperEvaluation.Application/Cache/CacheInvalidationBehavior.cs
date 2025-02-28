using Ambev.DeveloperEvaluation.Common.Cache;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cache;

public class CacheInvalidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ICacheService _cache;

    public CacheInvalidationBehavior(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Executa o handler
        var response = await next();

        // Invalida cache de queries relacionadas
        await _cache.RemoveAsync($"Cache_{typeof(TRequest).Name}");

        return response;
    }
}
