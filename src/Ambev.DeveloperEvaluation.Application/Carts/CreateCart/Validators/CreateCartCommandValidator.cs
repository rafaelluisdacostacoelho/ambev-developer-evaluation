using Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Validators;

/// <summary>
/// Validator for <see cref="CreateCartCommand"/> that defines validation rules for the cart creation process.
/// Ensures required fields are populated, formats are correct, and business logic constraints are met.
/// </summary>
public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCartCommandValidator"/> with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - <see cref="CreateCartCommand.UserId"/>: Must be a valid GUID and not empty.
    /// - <see cref="CreateCartCommand.Date"/>: Required and cannot be set to a future date.
    /// - <see cref="CreateCartCommand.Products"/>: Must not be empty and should contain valid products.
    /// 
    /// Each product in the list is validated using the <see cref="CreateCartItemCommandValidator"/> 
    /// to ensure that product-specific rules are followed, including valid product ID and quantity constraints.
    /// </remarks>
    public CreateCartCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .NotEqual(Guid.Empty).WithMessage("UserId must be a valid GUID.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Date cannot be in the future.");

        RuleFor(x => x.Products)
             .NotEmpty().WithMessage("Products list cannot be empty.")
             .Must(p => p != null && p.Count > 0).WithMessage("Products list must contain at least one item.")
             .ForEach(product =>
             {
                 product.NotNull().WithMessage("Product cannot be null.");
                 product.SetValidator(new CreateCartItemCommandValidator());
             });
    }
}
