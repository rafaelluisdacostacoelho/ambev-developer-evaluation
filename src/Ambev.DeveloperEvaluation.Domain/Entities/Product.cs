using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product in the system.
/// </summary>
public class Product : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string Image { get; private set; } = string.Empty;
    public CategoryInfo Category { get; private set; } = default!;
    public RatingInfo Rating { get; private set; } = default!;

    // Construtor privado para ORMs
    private Product() { }

    public Product(string title, decimal price, string description, string image, CategoryInfo category, RatingInfo rating)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be a positive value.");

        Title = title;
        Price = price;
        Description = description;
        Image = image;
        Category = category ?? throw new ArgumentNullException(nameof(category));
        Rating = rating ?? throw new ArgumentNullException(nameof(rating));
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(newPrice), "Price must be a positive value.");

        Price = newPrice;
    }

    public void UpdateCategory(CategoryInfo newCategory)
    {
        Category = newCategory ?? throw new ArgumentNullException(nameof(newCategory));
    }

    public void UpdateRating(RatingInfo newRating)
    {
        Rating = newRating ?? throw new ArgumentNullException(nameof(newRating));
    }
}
