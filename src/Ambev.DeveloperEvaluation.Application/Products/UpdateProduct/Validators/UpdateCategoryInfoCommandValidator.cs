using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct.Validators;

/// <summary>
/// Validator for the CreateRatingInfoCommand
/// </summary>
public class UpdateCategoryInfoCommandValidator : AbstractValidator<UpdateCategoryInfoCommand>
{
    /// <summary>
    /// Initializes validation rules for CreateRatingInfoCommand
    /// </summary>
    public UpdateCategoryInfoCommandValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty().WithMessage("ExternalId is required.")
            .MaximumLength(255).WithMessage("ExternalId must not exceed 255 characters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("ExternalId must not exceed 255 characters.");
    }
}
