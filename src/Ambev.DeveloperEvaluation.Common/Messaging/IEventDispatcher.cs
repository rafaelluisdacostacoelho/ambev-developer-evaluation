namespace Ambev.DeveloperEvaluation.Common.Messaging;

public interface IEventDispatcher<TEvent>
{
    IEnumerable<TEvent> GetDomainEvents();
}
