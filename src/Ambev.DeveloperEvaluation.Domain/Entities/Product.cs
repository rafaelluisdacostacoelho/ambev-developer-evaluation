using Ambev.DeveloperEvaluation.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product in the system.
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// The title of the product.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The price of the product.
    /// </summary>
    [Required]
    public decimal Price { get; set; }

    /// <summary>
    /// The product description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The URL of the product image.
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// Category as a Value Object (denormalized).
    /// </summary>
    [Required]
    public CategoryInfo Category { get; set; } = new CategoryInfo();

    /// <summary>
    /// Rating as a Value Object (denormalized).
    /// </summary>
    [Required]
    public RatingInfo Rating { get; set; } = new RatingInfo();
}
