using Ambev.DeveloperEvaluation.Application.Products.CreateProduct.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct.Validators;

/// <summary>
/// Validator for CreateProductCommand that defines validation rules for product creation command.
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateProductCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Title: Must not be empty.
    /// - Price: Must be greater than or equal to 0.
    /// - Description: Must not be empty.
    /// - Image: Must be a valid URL if provided.
    /// - Category: Must not be null and should be valid.
    /// - Rating: Must not be null and should be valid.
    /// </remarks>
    public CreateProductCommandValidator()
    {
        // Validates that the Title property is not empty.
        RuleFor(product => product.Title)
            .NotEmpty()
            .WithMessage("The product title must not be empty.");

        // Validates that the Price is a non-negative value.
        RuleFor(product => product.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("The product price must be greater than or equal to 0.");

        // Validates that the Description property is not empty.
        RuleFor(product => product.Description)
            .NotEmpty()
            .WithMessage("The product description must not be empty.");

        // Validates that the Image is a valid URL if provided.
        RuleFor(product => product.Image)
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .When(product => !string.IsNullOrWhiteSpace(product.Image))
            .WithMessage("The product image must be a valid URL.");

        // Validates that the Category object is not null and valid.
        RuleFor(product => product.Category)
            .NotNull()
            .WithMessage("The product category must not be null.")
            .SetValidator(new CreateCategoryInfoCommandValidator());

        // Validates that the Rating object is not null and valid.
        RuleFor(product => product.Rating)
            .NotNull()
            .WithMessage("The product rating must not be null.")
            .SetValidator(new CreateRatingInfoCommandValidator());
    }
}
