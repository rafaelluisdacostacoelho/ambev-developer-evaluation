using Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Validators;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart.Validators;

/// <summary>
/// Validator for <see cref="UpdateCartCommand"/> that defines validation rules for the cart creation process.
/// Ensures required fields are populated, formats are correct, and business logic constraints are met.
/// </summary>
public class UpdateCartCommandValidator : AbstractValidator<UpdateCartCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCartCommandValidator"/> with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - <see cref="UpdateCartCommand.UserId"/>: Must be a valid GUID and not empty.
    /// - <see cref="UpdateCartCommand.Date"/>: Required and cannot be set to a future date.
    /// - <see cref="UpdateCartCommand.Updates"/>: Must not be empty and should contain valid carts.
    /// 
    /// Each cart in the list is validated using the <see cref="UpdateCartItemCommandValidator"/> 
    /// to ensure that cart-specific rules are followed, including valid cart ID and quantity constraints.
    /// </remarks>
    public UpdateCartCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .NotEqual(Guid.Empty).WithMessage("UserId must be a valid GUID.");

        RuleFor(x => x.Products)
             .NotEmpty().WithMessage("Carts list cannot be empty.")
             .Must(p => p != null && p.Count > 0).WithMessage("Carts list must contain at least one item.")
             .ForEach(cart =>
             {
                 cart.NotNull().WithMessage("Cart cannot be null.");
                 cart.SetValidator(new UpdateCartItemCommandValidator());
             });
    }
}
