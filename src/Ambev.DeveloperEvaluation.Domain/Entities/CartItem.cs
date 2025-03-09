namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class CartItem
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal PriceTotal { get; private set; }
    public decimal PriceTotalWithDiscount { get; private set; }
    public decimal Discount { get; private set; }

    private CartItem() { }

    public CartItem(Guid productId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        UnitPrice = unitPrice;
        UpdateProductQuantity(quantity, unitPrice);
    }

    public void UpdateProductQuantity(int quantity, decimal unitPrice)
    {
        if (quantity < 1 || 20 < quantity)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be between 1 and 20");

        Quantity = quantity;
        UnitPrice = unitPrice;
        CalculateDiscountedPrice(Quantity, unitPrice);
    }

    private void CalculateDiscountedPrice(int quantity, decimal unitPrice)
    {
        Discount = 0m;

        if (quantity >= 4 && quantity < 10)
        {
            Discount = 0.10m;
        }
        else if (quantity >= 10 && quantity <= 20)
        {
            Discount = 0.20m;
        }

        PriceTotal = unitPrice * quantity;
        PriceTotalWithDiscount = PriceTotal - (PriceTotal * Discount);
    }
}
