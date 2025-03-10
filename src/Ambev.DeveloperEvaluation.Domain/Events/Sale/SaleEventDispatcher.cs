using Ambev.DeveloperEvaluation.Common.Messaging;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale;

public class SaleEventDispatcher : IEventDispatcher<SaleCreatedEvent>
{
    private readonly List<SaleCreatedEvent> _events = [];

    public void AddEvent(SaleCreatedEvent saleEvent)
    {
        _events.Add(saleEvent);
    }

    public IEnumerable<SaleCreatedEvent> GetDomainEvents()
    {
        return _events;
    }
}