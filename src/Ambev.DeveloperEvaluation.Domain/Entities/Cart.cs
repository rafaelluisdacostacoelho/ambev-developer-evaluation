using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Cart : BaseEntity
{
    public Guid UserId { get; private set; }
    public DateTime Date { get; private set; }
    public List<CartItem> Products { get; private set; } = [];

    // Construtor privado para ORMs
    // Cart possui construtor privado para evitar inicializações inválidas e garantir imutabilidade.
    private Cart() { }

    public Cart(Guid userId)
    {
        UserId = userId;
        Date = DateTime.UtcNow;
        Products = [];
    }

    public void AddProduct(Guid productId, int quantity)
    {
        if (quantity < 1 || quantity > 20)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be between 1 and 20");

        var existingItem = Products.FirstOrDefault(p => p.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            Products.Add(new CartItem(productId, quantity));
        }
    }

    public void RemoveProduct(Guid productId)
    {
        Products.RemoveAll(p => p.ProductId == productId);
    }
}
