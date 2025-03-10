using Ambev.DeveloperEvaluation.Common.Messaging;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale.Commands;

[DispatchDomainEvents]
public class CreateSaleCommand : IRequest<Guid>
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public bool IsCancelled { get; set; }

    // External ID do Cart
    public Guid CartId { get; set; }

    // Campos desnormalizados do Cart
    public Guid UserId { get; set; }
}
