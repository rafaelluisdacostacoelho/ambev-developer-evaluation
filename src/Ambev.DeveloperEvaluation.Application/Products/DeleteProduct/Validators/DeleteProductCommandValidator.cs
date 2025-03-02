using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct.Validators;

/// <summary>
/// Validator for DeleteProductCommand
/// </summary>
public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    /// <summary>
    /// Initializes validation rules for DeleteProductCommand
    /// </summary>
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
    }
}
