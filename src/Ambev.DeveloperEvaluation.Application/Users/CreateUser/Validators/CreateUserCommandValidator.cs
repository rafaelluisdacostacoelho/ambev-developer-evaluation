using Ambev.DeveloperEvaluation.Application.Users.CreateUser.Commands;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser.Validators;

/// <summary>
/// Validator for CreateUserCommand that defines validation rules for user creation command.
/// Ensures required fields, format correctness, and valid business logic constraints.
/// </summary>
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateUserCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Email: Must be in valid format (using EmailValidator).
    /// - Username: Required, must be between 3 and 50 characters.
    /// - Password: Must meet security requirements (using PasswordValidator).
    /// - Phone: Must match international format (+X XXXXXXXXXX).
    /// - Status: Cannot be set to Unknown.
    /// - Role: Cannot be set to None.
    /// - Name: Firstname and Lastname are required and must be within the character limit.
    /// - Address: Street, City, and Zipcode must be valid.
    /// - Geolocation: Latitude and Longitude are required.
    /// </remarks>
    public CreateUserCommandValidator()
    {
        // Validate Email (using a predefined custom validator)
        RuleFor(user => user.Email)
            .SetValidator(new EmailValidator());

        // Validate Username (required, length between 3 and 50 characters)
        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.");

        // Validate Password (using a predefined custom validator)
        RuleFor(user => user.Password)
            .SetValidator(new PasswordValidator());

        // Validate Phone number (must follow international format +X XXXXXXXXXX)
        RuleFor(user => user.Phone)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number must be in international format (e.g., +1 1234567890).");

        // Validate User Status (must not be Unknown)
        RuleFor(user => user.Status)
            .NotEqual(UserStatus.Unknown)
            .WithMessage("User status cannot be Unknown.");

        // Validate User Role (must not be None)
        RuleFor(user => user.Role)
            .NotEqual(UserRole.None)
            .WithMessage("User role cannot be None.");

        // ===== Name Validation =====
        RuleFor(user => user.Name.Firstname)
            .NotEmpty().WithMessage("Firstname is required.")
            .MaximumLength(100).WithMessage("Firstname cannot exceed 100 characters.");

        RuleFor(user => user.Name.Lastname)
            .NotEmpty().WithMessage("Lastname is required.")
            .MaximumLength(100).WithMessage("Lastname cannot exceed 100 characters.");

        // ===== Address Validation =====
        RuleFor(user => user.Address.Street)
            .NotEmpty().WithMessage("Street address is required.")
            .MaximumLength(100).WithMessage("Street address cannot exceed 100 characters.");

        RuleFor(user => user.Address.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(100).WithMessage("City name cannot exceed 100 characters.");

        RuleFor(user => user.Address.Zipcode)
            .NotEmpty().WithMessage("Zipcode is required.")
            .Matches(@"^\d{5}-\d{3}$")
            .WithMessage("Zipcode must be in the format XXXXX-XXX (e.g., 12345-678).");

        // ===== Geolocation Validation =====
        RuleFor(user => user.Address.Geolocation.Latitude)
            .NotEmpty().WithMessage("Latitude is required.");

        RuleFor(user => user.Address.Geolocation.Longitude)
            .NotEmpty().WithMessage("Longitude is required.");
    }
}
