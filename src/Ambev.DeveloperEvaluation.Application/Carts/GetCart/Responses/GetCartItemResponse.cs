namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart.Responses;

public class GetCartItemResponse
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
