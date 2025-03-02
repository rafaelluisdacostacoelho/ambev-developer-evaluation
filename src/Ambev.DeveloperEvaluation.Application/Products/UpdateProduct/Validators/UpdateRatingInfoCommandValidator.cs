using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct.Validators;

/// <summary>
/// Validator for the UpdateRatingInfoCommand
/// </summary>
public class UpdateRatingInfoCommandValidator : AbstractValidator<UpdateRatingInfoCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateRatingInfoCommand
    /// </summary>
    public UpdateRatingInfoCommandValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty().WithMessage("ExternalId is required.")
            .MaximumLength(255).WithMessage("ExternalId must not exceed 255 characters.");

        RuleFor(x => x.AverageRate)
            .InclusiveBetween(0.0, 5.0).WithMessage("AverageRate must be between 0.0 and 5.0.");

        RuleFor(x => x.TotalReviews)
            .GreaterThanOrEqualTo(0).WithMessage("TotalReviews cannot be negative.");
    }
}
