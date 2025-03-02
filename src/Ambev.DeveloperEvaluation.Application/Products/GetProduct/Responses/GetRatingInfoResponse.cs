namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct.Responses;

public class GetRatingInfoResponse
{
    public string ExternalId { get; private set; } = string.Empty;
    public double AverageRate { get; private set; }
    public int TotalReviews { get; private set; }
}
