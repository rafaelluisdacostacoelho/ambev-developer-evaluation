using MediatR;

namespace Ambev.DeveloperEvaluation.Common.Messaging;

public class DomainEventPublisherBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class
{
    private readonly IDomainEventDispatcher _eventDispatcher;
    private readonly IEventPublisher _eventPublisher;

    public DomainEventPublisherBehavior(IDomainEventDispatcher eventDispatcher, IEventPublisher eventPublisher)
    {
        _eventDispatcher = eventDispatcher;
        _eventPublisher = eventPublisher;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        // Verifica se o comando possui o atributo [DispatchDomainEvents]
        var shouldDispatchEvents = request.GetType().GetCustomAttributes(typeof(DispatchDomainEventsAttribute), true).Length > 0;

        if (shouldDispatchEvents)
        {
            foreach (var domainEvent in _eventDispatcher.GetDomainEvents())
            {
                await _eventPublisher.PublishEventAsync(domainEvent);
            }
        }

        _eventDispatcher.ClearEvents();
        return response;
    }
}
