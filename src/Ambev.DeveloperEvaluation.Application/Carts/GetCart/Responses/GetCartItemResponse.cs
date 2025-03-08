namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart.Responses;

public class GetCartItemResponse
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal PriceTotal { get; set; }
    public decimal PriceTotalWithDiscount { get; set; }
    public decimal Discount { get; set; }
}
