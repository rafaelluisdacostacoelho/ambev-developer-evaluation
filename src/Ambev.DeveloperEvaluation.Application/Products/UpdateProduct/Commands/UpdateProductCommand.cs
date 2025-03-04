using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct.Responses;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct.Validators;
using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct.Commands;

/// <summary>
/// Command to create a new product
/// </summary>
public class UpdateProductCommand : IRequest<UpdateProductResponse>
{
    public Guid Id { get; set; }

    /// <summary>
    /// The product's title
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// The product's price
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// The product's description
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// The product's image URL or path
    /// </summary>
    public string Image { get; private set; } = string.Empty;

    /// <summary>
    /// The category information for the product
    /// </summary>
    public UpdateCategoryInfoCommand Category { get; private set; } = default!;

    /// <summary>
    /// The rating information for the product
    /// </summary>
    public UpdateRatingInfoCommand Rating { get; private set; } = default!;

    public UpdateProductCommand(string title, decimal price, string description, string image, UpdateCategoryInfoCommand category, UpdateRatingInfoCommand rating)
    {
        Title = title;
        Price = price;
        Description = description;
        Image = image;
        Category = category;
        Rating = rating;
    }

    /// <summary>
    /// Validates the CreateProductCommand using the CreateProductCommandValidator
    /// </summary>
    /// <returns>A ValidationResultDetail containing validation results</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new UpdateProductCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(error => (ValidationErrorDetail)error)
        };
    }
}
