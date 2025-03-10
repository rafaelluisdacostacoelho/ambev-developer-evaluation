using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Common.Messaging;

public class DomainEventPublisherBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class
{
    private readonly IDomainEventDispatcher<object> _eventDispatcher;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<DomainEventPublisherBehavior<TRequest, TResponse>> _logger;

    public DomainEventPublisherBehavior(
        IDomainEventDispatcher<object> eventDispatcher,
        IEventPublisher eventPublisher,
        ILogger<DomainEventPublisherBehavior<TRequest, TResponse>> logger)
    {
        _eventDispatcher = eventDispatcher;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        var shouldDispatchEvents = Attribute.IsDefined(request.GetType(), typeof(DispatchDomainEventsAttribute));

        if (shouldDispatchEvents)
        {
            // Busca automaticamente propriedades do tipo IEvent no request
            var domainEvents = request.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => typeof(IEvent).IsAssignableFrom(prop.PropertyType))
                .Select(prop => prop.GetValue(request))
                .OfType<IEvent>()
                .ToList();


            foreach (var domainEvent in _eventDispatcher.GetDomainEvents())
            {
                try
                {
                    await _eventPublisher.PublishEventAsync(domainEvent);
                    _logger.LogInformation("Evento publicado: {EventName}", domainEvent.GetType().Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao publicar evento: {EventName}", domainEvent.GetType().Name);
                }
            }
        }

        _eventDispatcher.ClearEvents();
        return response;
    }
}
