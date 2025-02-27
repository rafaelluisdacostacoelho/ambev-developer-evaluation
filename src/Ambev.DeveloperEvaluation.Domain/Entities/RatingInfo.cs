using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Value Object representing the product rating information.
/// </summary>
[Owned]
public class RatingInfo
{
    /// <summary>
    /// External rating identifier.
    /// </summary>
    [Required]
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Average rating score (e.g., 4.5).
    /// </summary>
    [Required]
    [Range(0, 5)]
    public double AverageRate { get; set; }

    /// <summary>
    /// Total number of reviews.
    /// </summary>
    [Required]
    public int TotalReviews { get; set; }
}
