using MediatR;

namespace Ambev.DeveloperEvaluation.Common.Messaging;

public class EventPublishingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class
{
    private readonly IEventPublisher _eventPublisher;

    public EventPublishingBehavior(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if (request is IEventDispatcher<TRequest> domainEventRequest)
        {
            foreach (var domainEvent in domainEventRequest.GetDomainEvents())
            {
                await _eventPublisher.PublishEventAsync(domainEvent);
            }
        }

        return response;
    }
}
