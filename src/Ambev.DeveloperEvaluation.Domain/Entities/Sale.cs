using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; private set; } = string.Empty;
    public DateTime SaleDate { get; private set; }
    public string Customer { get; private set; } = string.Empty;
    public string Branch { get; private set; } = string.Empty;
    public bool IsCancelled { get; private set; }

    // External ID do Cart
    public Guid CartId { get; private set; }

    // Campos desnormalizados do Cart
    public Guid UserId { get; private set; }
    public decimal PriceTotal { get; private set; }

    private Sale() { }

    public Sale(Guid id, string saleNumber, DateTime saleDate, string customer, string branch, bool isCancelled, Guid cartId, Guid userId, decimal priceTotal)
    {
        if (string.IsNullOrWhiteSpace(saleNumber)) throw new ArgumentException("Sale number is required.", nameof(saleNumber));
        if (string.IsNullOrWhiteSpace(customer)) throw new ArgumentException("Customer is required.", nameof(customer));
        if (string.IsNullOrWhiteSpace(branch)) throw new ArgumentException("Branch is required.", nameof(branch));

        Id = id;
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        Customer = customer;
        Branch = branch;
        IsCancelled = isCancelled;
        CartId = cartId;
        UserId = userId;
        PriceTotal = priceTotal;
    }

    public void UpdatePriceTotal(decimal priceTotal)
    {
        PriceTotal = priceTotal;
    }
}
