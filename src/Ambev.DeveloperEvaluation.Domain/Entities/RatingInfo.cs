namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Value Object representing the product rating information.
/// </summary>
public class RatingInfo
{
    public string ExternalId { get; private set; } = string.Empty;
    public double AverageRate { get; private set; }
    public int TotalReviews { get; private set; }

    private RatingInfo() { }

    public RatingInfo(string externalId, double averageRate, int totalReviews)
    {
        if (string.IsNullOrWhiteSpace(externalId))
            throw new ArgumentException("ExternalId cannot be empty.", nameof(externalId));

        if (averageRate < 0 || averageRate > 5)
            throw new ArgumentOutOfRangeException(nameof(averageRate), "AverageRate must be between 0 and 5.");

        if (totalReviews < 0)
            throw new ArgumentOutOfRangeException(nameof(totalReviews), "TotalReviews must be a positive number.");

        ExternalId = externalId;
        AverageRate = averageRate;
        TotalReviews = totalReviews;
    }

    public void UpdateRating(double newRate, int newTotalReviews)
    {
        if (newRate < 0 || newRate > 5)
            throw new ArgumentOutOfRangeException(nameof(newRate), "AverageRate must be between 0 and 5.");

        if (newTotalReviews < 0)
            throw new ArgumentOutOfRangeException(nameof(newTotalReviews), "TotalReviews must be a positive number.");

        AverageRate = newRate;
        TotalReviews = newTotalReviews;
    }
}