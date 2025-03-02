namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart.Responses;

/// <summary>
/// Response model for GetCart operation
/// </summary>
public class GetCartResponse
{
    public Guid UserId { get; private set; }
    public DateTime Date { get; private set; }
    public List<GetCartItemResponse> Products { get; private set; } = [];

}
