namespace Ambev.DeveloperEvaluation.Domain.Events.Sale;

public class SaleCreatedEvent
{
    public Guid SaleId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}