using Ambev.DeveloperEvaluation.Common.Messaging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale.Events;

public class SaleCreatedEvent : IEvent
{
    public Guid SaleId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public SaleCreatedEvent(Guid saleId)
    {
        SaleId = saleId;
    }
}