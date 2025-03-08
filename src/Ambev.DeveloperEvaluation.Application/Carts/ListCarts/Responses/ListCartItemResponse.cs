namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts.Responses;

public class ListCartItemResponse
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal PriceTotal { get; set; }
    public decimal PriceTotalWithDiscount { get; set; }
    public decimal Discount { get; set; }
}
