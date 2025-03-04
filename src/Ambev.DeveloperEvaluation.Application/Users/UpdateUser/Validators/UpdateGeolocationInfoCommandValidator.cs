using Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Validators;

/// <summary>
/// Validator for the UpdateGeolocationInfoCommand
/// </summary>
public class UpdateGeolocationInfoCommandValidator : AbstractValidator<UpdateGeolocationInfoCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateGeolocationInfoCommand
    /// </summary>
    public UpdateGeolocationInfoCommandValidator()
    {
        // Validação da Latitude
        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90.");

        // Validação da Longitude
        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180.");
    }
}
