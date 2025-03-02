namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts.Responses;

/// <summary>
/// Response model for GetCart operation
/// </summary>
public class ListCartResponse
{
    public Guid UserId { get; private set; }
    public DateTime Date { get; private set; }
    public ICollection<ListCartItemResponse> Products { get; private set; } = [];

}
