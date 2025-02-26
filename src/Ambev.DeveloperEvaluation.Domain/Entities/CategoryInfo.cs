using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Value Object of Category
/// </summary>
[Owned]
public class CategoryInfo
{
    /// <summary>
    /// External ID
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Denormalized Name
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
