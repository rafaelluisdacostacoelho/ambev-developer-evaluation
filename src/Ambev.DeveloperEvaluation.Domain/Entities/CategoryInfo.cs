using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Value Object representing the category of a product.
/// </summary>
[Owned]
public class CategoryInfo
{
    /// <summary>
    /// External category identifier.
    /// </summary>
    [Required]
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Denormalized category name.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;
}
