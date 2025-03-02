namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart.Responses;

public class GetCartItemResponse
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
}
