namespace Ambev.DeveloperEvaluation.Common.Messaging;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly List<object> _domainEvents = [];

    public void AddEvent<TEvent>(TEvent domainEvent) where TEvent : class
    {
        _domainEvents.Add(domainEvent);
    }

    public IEnumerable<object> GetDomainEvents() => _domainEvents;

    public void ClearEvents() => _domainEvents.Clear();
}
