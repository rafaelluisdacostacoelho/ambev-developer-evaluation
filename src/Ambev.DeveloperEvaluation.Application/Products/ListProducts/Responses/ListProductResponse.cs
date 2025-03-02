
namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts.Responses;

public class ListProductResponse
{
    public string Title { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string Image { get; private set; } = string.Empty;
    public ListProductCategoryInfoResponse Category { get; private set; } = default!;
    public ListProductRatingInfoResponse Rating { get; private set; } = default!;
}
