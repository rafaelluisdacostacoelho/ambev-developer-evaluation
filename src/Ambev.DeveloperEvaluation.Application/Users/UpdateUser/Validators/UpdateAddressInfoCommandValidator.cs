using Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Validators;

/// <summary>
/// Validator for the UpdateAddressInfoCommand
/// </summary>
public class UpdateAddressInfoCommandValidator : AbstractValidator<UpdateAddressInfoCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateAddressInfoCommand
    /// </summary>
    public UpdateAddressInfoCommandValidator()
    {
        // Validação da Cidade
        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(255).WithMessage("City must not exceed 255 characters.");

        // Validação da Rua
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(255).WithMessage("Street must not exceed 255 characters.");

        // Validação do Número
        RuleFor(x => x.Number)
            .GreaterThan(0).WithMessage("Number must be greater than 0.");

        // Validação do CEP (Zipcode)
        RuleFor(x => x.Zipcode)
            .NotEmpty().WithMessage("Zipcode is required.")
            .Matches(@"^\d{5}-\d{3}$").WithMessage("Zipcode must be in the format 12345-678.");

        // Validação da Geolocalização (Utilizando o Validator específico)
        RuleFor(x => x.Geolocation)
            .NotNull().WithMessage("Geolocation information is required.")
            .SetValidator(new UpdateGeolocationInfoCommandValidator());
    }
}
