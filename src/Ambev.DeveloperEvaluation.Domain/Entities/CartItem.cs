namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class CartItem
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public decimal Discount { get; private set; }

    private CartItem() { }

    public CartItem(Guid productId, int quantity)
    {
        if (quantity < 1 || quantity > 20)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be between 1 and 20");

        ProductId = productId;
        Quantity = quantity;
    }

    public void IncreaseQuantity(int quantity, decimal unitPrice)
    {
        if (Quantity + quantity > 20)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Total quantity cannot exceed 20");

        Quantity += quantity;
        Price = CalculateDiscountedPrice(Quantity, unitPrice);
    }

    public void DecreaseQuantity(int quantity, decimal unitPrice)
    {
        if (Quantity - quantity < 1)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least 1");

        Quantity -= quantity;
        Price = CalculateDiscountedPrice(Quantity, unitPrice);
    }

    private decimal CalculateDiscountedPrice(int quantity, decimal unitPrice)
    {
        const decimal noDiscount = 0m;
        const decimal discountOf10percent = 0.10m;
        const decimal discountOf20percent = 0.20m;

        if (quantity < 4)
        {
            return unitPrice * quantity;
        }

        decimal discount = noDiscount;

        if (quantity >= 4 && quantity < 10)
        {
            discount = discountOf10percent;
        }
        else if (quantity >= 10 && quantity <= 20)
        {
            discount = discountOf20percent;
        }

        Discount = discount;

        decimal totalPrice = unitPrice * quantity;
        return totalPrice - (totalPrice * discount);
    }
}
