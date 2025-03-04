using Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Validators;

public class CreateCartItemCommandValidator : AbstractValidator<CreateCartItemCommand>
{
    public CreateCartItemCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.")
            .NotEqual(Guid.Empty).WithMessage("ProductId must be a valid GUID.");

        RuleFor(x => x.Quantity)
            .InclusiveBetween(1, 20).WithMessage("Quantity must be between 1 and 20.");
    }
}
