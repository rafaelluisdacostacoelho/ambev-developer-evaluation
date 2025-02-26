using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// 
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// 
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// Categoria como Value Object (denormalizado) 
    /// </summary>
    public CategoryInfo Category { get; set; } = new CategoryInfo();

    /// <summary>
    /// Avaliação como Value Object (denormalizado)
    /// </summary>
    public RatingInfo Rating { get; set; } = new RatingInfo();
}
