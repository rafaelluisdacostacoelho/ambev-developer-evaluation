using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Cart : BaseEntity
{
    public Guid UserId { get; private set; }
    public DateTime Date { get; private set; }
    public decimal TotalPrice { get; private set; }
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

    public void UpdateProduct(Guid productId, int quantity, decimal unitPrice)
    {
        if (quantity < 1 || quantity > 20)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be between 1 and 20");

        var existingItem = Products.FirstOrDefault(p => p.ProductId == productId);
        if (existingItem != null)
        {
            var oldPrice = existingItem.Price;
            if (existingItem.Quantity < quantity)
            {
                existingItem.IncreaseQuantity(quantity - existingItem.Quantity, unitPrice);
            }
            else
            {
                existingItem.DecreaseQuantity(existingItem.Quantity - quantity, unitPrice);
            }
            var newPrice = existingItem.Price;

            // Atualiza o total apenas pela diferença de preço
            TotalPrice += newPrice - oldPrice;
        }
        else
        {
            var newItem = new CartItem(productId, quantity);

            newItem.IncreaseQuantity(quantity, unitPrice);

            Products.Add(newItem);

            // Adiciona o preço do novo item ao total
            TotalPrice += newItem.Price;
        }
    }

    public void RemoveProduct(Guid productId)
    {
        var itemToRemove = Products.FirstOrDefault(p => p.ProductId == productId);
        if (itemToRemove != null)
        {
            // Subtrai o preço do item do total antes de remover
            TotalPrice -= itemToRemove.Price;
            Products.Remove(itemToRemove);
        }
    }
}
