using Ambev.DeveloperEvaluation.Application.Users.GetUser.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser.Validators;

/// <summary>
/// Validator for GetUserCommand
/// </summary>
public class GetUserCommandValidator : AbstractValidator<GetUserCommand>
{
    /// <summary>
    /// Initializes validation rules for GetUserCommand
    /// </summary>
    public GetUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("User ID is required");
    }
}
