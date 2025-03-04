using Ambev.DeveloperEvaluation.Application.Products.CreateProduct.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct.Validators;

/// <summary>
/// Validator for the CreateRatingInfoCommand
/// </summary>
public class CreateCategoryInfoCommandValidator : AbstractValidator<CreateCategoryInfoCommand>
{
    /// <summary>
    /// Initializes validation rules for CreateRatingInfoCommand
    /// </summary>
    public CreateCategoryInfoCommandValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty().WithMessage("ExternalId is required.")
            .MaximumLength(255).WithMessage("ExternalId must not exceed 255 characters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("ExternalId must not exceed 255 characters.");
    }
}
