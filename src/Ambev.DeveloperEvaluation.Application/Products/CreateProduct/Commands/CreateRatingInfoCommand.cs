namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct.Commands;

/// <summary>
/// Command to provide rating information for a product
/// </summary>
public class CreateRatingInfoCommand
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

    public CreateRatingInfoCommand(string externalId, double averageRate, int totalReviews)
    {
        ExternalId = externalId;
        AverageRate = averageRate;
        TotalReviews = totalReviews;
    }
}
