using Ambev.DeveloperEvaluation.Application.Products.GetProduct.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct.Validators;

/// <summary>
/// Validator for GetProductCommand
/// </summary>
public class GetProductCommandValidator : AbstractValidator<GetProductCommand>
{
    /// <summary>
    /// Initializes validation rules for GetProductCommand
    /// </summary>
    public GetProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");
    }
}
