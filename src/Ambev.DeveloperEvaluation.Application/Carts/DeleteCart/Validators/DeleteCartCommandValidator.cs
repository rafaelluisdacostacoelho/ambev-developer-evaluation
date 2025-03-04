using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart.Validators;

/// <summary>
/// Validator for DeleteCartCommand
/// </summary>
public class DeleteCartCommandValidator : AbstractValidator<DeleteCartCommand>
{
    /// <summary>
    /// Initializes validation rules for DeleteCartCommand
    /// </summary>
    public DeleteCartCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Cart ID is required");
    }
}
