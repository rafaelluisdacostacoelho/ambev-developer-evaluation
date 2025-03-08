namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts.Responses;

/// <summary>
/// Response model for GetCart operation
/// </summary>
public class ListCartResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public decimal PriceTotal { get; set; }
    public ICollection<ListCartItemResponse> Products { get; set; } = [];
}
