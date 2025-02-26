using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Cart : BaseEntity
{
    public Guid UserId { get; set; } // Referência ao usuário
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public List<CartItem> Products { get; set; } = [];
}
