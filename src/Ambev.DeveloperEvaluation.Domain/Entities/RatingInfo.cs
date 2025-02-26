using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

[Owned]
public class RatingInfo
{
    public string ExternalId { get; set; } = string.Empty;
    public double AverageRate { get; set; }
    public int TotalReviews { get; set; }
}