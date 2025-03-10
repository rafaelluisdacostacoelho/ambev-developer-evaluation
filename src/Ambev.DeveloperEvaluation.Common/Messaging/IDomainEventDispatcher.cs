namespace Ambev.DeveloperEvaluation.Common.Messaging;

public interface IDomainEventDispatcher
{
    void AddEvent<TEvent>(TEvent domainEvent) where TEvent : class;
    IEnumerable<object> GetDomainEvents();
    void ClearEvents();
}