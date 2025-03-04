namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart.Commands;

public class UpdateCartItemCommand
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    public UpdateCartItemCommand(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}
