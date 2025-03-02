namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class CartItem
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    // Construtor privado para ORMs
    // CartItem possui construtor privado para evitar inicializações inválidas e garantir imutabilidade.
    private CartItem() { }

    public CartItem(Guid productId, int quantity)
    {
        if (quantity < 1 || quantity > 20)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be between 1 and 20");

        ProductId = productId;
        Quantity = quantity;
    }

    public void IncreaseQuantity(int amount)
    {
        if (Quantity + amount > 20)
            throw new ArgumentOutOfRangeException(nameof(amount), "Total quantity cannot exceed 20");

        Quantity += amount;
    }

    public void DecreaseQuantity(int amount)
    {
        if (Quantity - amount < 1)
            throw new ArgumentOutOfRangeException(nameof(amount), "Quantity must be at least 1");

        Quantity -= amount;
    }
}
