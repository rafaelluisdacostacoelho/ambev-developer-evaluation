namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts.Responses;

public class ListProductRatingInfoResponse
{
    public string ExternalId { get; set; } = string.Empty;
    public double AverageRate { get; set; }
    public int TotalReviews { get; set; }
}
