namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct.Commands;

/// <summary>
/// Command to provide rating information for a product
/// </summary>
public class UpdateRatingInfoCommand
{
    /// <summary>
    /// The external identifier for the rating source
    /// </summary>
    public string ExternalId { get; private set; } = string.Empty;

    /// <summary>
    /// The average rating of the product
    /// </summary>
    public double AverageRate { get; private set; }

    /// <summary>
    /// The total number of reviews for the product
    /// </summary>
    public int TotalReviews { get; private set; }

    public UpdateRatingInfoCommand(string externalId, double averageRate, int totalReviews)
    {
        ExternalId = externalId;
        AverageRate = averageRate;
        TotalReviews = totalReviews;
    }
}
