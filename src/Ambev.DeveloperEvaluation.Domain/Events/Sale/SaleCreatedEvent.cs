using Ambev.DeveloperEvaluation.Common.Messaging;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale;

public class SaleCreatedEvent : IEvent
{
    public Guid SaleId { get; }
    public DateTime OccurredOn { get; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

    public SaleCreatedEvent(Guid saleId)
    {
        SaleId = saleId;
    }
}