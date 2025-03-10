namespace Ambev.DeveloperEvaluation.Common.Messaging;

public interface IDomainEventDispatcher<TEvent> where TEvent : class
{
    void AddEvent(TEvent domainEvent);
    IEnumerable<TEvent> GetDomainEvents();
    void ClearEvents();
}
