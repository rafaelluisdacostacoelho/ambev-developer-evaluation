namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Commands;

public class CreateCartItemCommand
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    public CreateCartItemCommand(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}
