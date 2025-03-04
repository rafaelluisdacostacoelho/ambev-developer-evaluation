using Ambev.DeveloperEvaluation.Application.Products.CreateProduct.Responses;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct.Validators;
using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct.Commands;

/// <summary>
/// Command to create a new product
/// </summary>
public class CreateProductCommand : IRequest<CreateProductResponse>
{
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
    public CreateCategoryInfoCommand Category { get; private set; } = default!;

    /// <summary>
    /// The rating information for the product
    /// </summary>
    public CreateRatingInfoCommand Rating { get; private set; } = default!;

    public CreateProductCommand(string title, decimal price, string description, string image, CreateCategoryInfoCommand category, CreateRatingInfoCommand rating)
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
        var validator = new CreateProductCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(error => (ValidationErrorDetail)error)
        };
    }
}
