namespace Ambev.DeveloperEvaluation.Common.Messaging;

public class DomainEventDispatcher<TEvent> : IDomainEventDispatcher<TEvent> where TEvent : class
{
    private readonly List<TEvent> _domainEvents = [];

    public void AddEvent(TEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IEnumerable<TEvent> GetDomainEvents() => _domainEvents;

    public void ClearEvents() => _domainEvents.Clear();
}
