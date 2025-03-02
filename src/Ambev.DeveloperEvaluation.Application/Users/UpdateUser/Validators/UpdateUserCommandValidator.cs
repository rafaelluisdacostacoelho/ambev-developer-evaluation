using Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Validators;

/// <summary>
/// Validator for UpdateUserCommand that defines validation rules for updating a user.
/// </summary>
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateUserCommandValidator with defined validation rules.
    /// </summary>
    public UpdateUserCommandValidator()
    {
        // Validação do ID
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID must not be empty.")
            .NotEqual(Guid.Empty).WithMessage("User ID must be a valid GUID.");

        // Validação do Username
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username must not be empty.")
            .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.");

        // Validação do Email
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email must not be empty.")
            .EmailAddress().WithMessage("Invalid email format.");

        // Validação do Telefone
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number must not be empty.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

        // Validação da Senha
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password must not be empty.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        // Validação do Papel do Usuário (Role)
        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("Invalid user role.");

        // Validação do Status do Usuário
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid user status.");

        // Validação do Nome do Usuário
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name information must not be null.")
            .SetValidator(new UpdateNameInfoCommandValidator());

        // Validação do Endereço do Usuário
        RuleFor(x => x.Address)
            .NotNull().WithMessage("Address information must not be null.")
            .SetValidator(new UpdateAddressInfoCommandValidator());
    }
}
