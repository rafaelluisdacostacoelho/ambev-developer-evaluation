namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct.Responses;

public class GetProductResponse
{
    public string Title { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string Image { get; private set; } = string.Empty;
    public GetCategoryInfoResponse Category { get; private set; } = default!;
    public GetRatingInfoResponse Rating { get; private set; } = default!;
}
