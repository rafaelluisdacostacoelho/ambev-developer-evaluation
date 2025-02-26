using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

[Owned]
public class CartItem
{
    public Guid ProductId { get; set; } // Referência ao produto
    public int Quantity { get; set; }
}
