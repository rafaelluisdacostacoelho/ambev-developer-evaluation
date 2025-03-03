using Ambev.DeveloperEvaluation.Domain.Validation;
using Bogus;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the EmailValidator class.
/// Tests cover various email validation scenarios including format, length, and empty checks.
/// </summary>
public class EmailValidatorTests
{
    private readonly EmailValidator _validator;

    public EmailValidatorTests()
    {
        _validator = new EmailValidator();
    }

    [Fact(DisplayName = "Valid email formats should pass validation")]
    public void Given_ValidEmailFormat_When_Validated_Then_ShouldNotHaveErrors()
    {
        var faker = new Faker();
        var email = faker.Internet.Email();

        var result = _validator.TestValidate(email);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Empty email should fail validation")]
    public void Given_EmptyEmail_When_Validated_Then_ShouldHaveError()
    {
        var email = string.Empty;

        var result = _validator.TestValidate(email);

        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("The email address cannot be empty.");
    }

    [Theory(DisplayName = "Invalid email formats should fail validation")]
    [InlineData("invalid-email")]
    [InlineData("user@")]
    [InlineData("@domain.com")]
    [InlineData("user@.com")]
    [InlineData("user@domain.")]
    public void Given_InvalidEmailFormat_When_Validated_Then_ShouldHaveError(string email)
    {
        var result = _validator.TestValidate(email);

        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("The provided email address is not valid.");
    }

    [Fact(DisplayName = "Email exceeding maximum length should fail validation")]
    public void Given_EmailExceeding100Characters_When_Validated_Then_ShouldHaveError()
    {
        var email = $"{"a".PadLeft(90, 'a')}@example.com";

        var result = _validator.TestValidate(email);

        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("The email address cannot be longer than 100 characters.");
    }
}