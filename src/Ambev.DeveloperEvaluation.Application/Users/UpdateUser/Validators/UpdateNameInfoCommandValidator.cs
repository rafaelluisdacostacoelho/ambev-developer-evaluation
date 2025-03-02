using Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Validators;

/// <summary>
/// Validator for the UpdateNameInfoCommand
/// </summary>
public class UpdateNameInfoCommandValidator : AbstractValidator<UpdateNameInfoCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateNameInfoCommand
    /// </summary>
    public UpdateNameInfoCommandValidator()
    {
        // Validação do Primeiro Nome (Firstname)
        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("Firstname is required.")
            .MaximumLength(100).WithMessage("Firstname must not exceed 100 characters.");

        // Validação do Último Nome (Lastname)
        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Lastname is required.")
            .MaximumLength(100).WithMessage("Lastname must not exceed 100 characters.");
    }
}
