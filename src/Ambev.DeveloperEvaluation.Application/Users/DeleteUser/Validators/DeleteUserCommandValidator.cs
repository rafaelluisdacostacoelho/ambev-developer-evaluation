using Ambev.DeveloperEvaluation.Application.Users.DeleteUser.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser.Validators;

/// <summary>
/// Validator for DeleteUserCommand
/// </summary>
public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    /// <summary>
    /// Initializes validation rules for DeleteUserCommand
    /// </summary>
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("User ID is required");
    }
}
