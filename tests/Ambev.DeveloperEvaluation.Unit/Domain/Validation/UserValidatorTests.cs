using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Bogus;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the UserValidator class.
/// Tests cover validation of all user properties including username, email,
/// password, phone, status, and role requirements.
/// </summary>
public class UserValidatorTests
{
    private readonly UserValidator _validator;
    private readonly Faker _faker;

    public UserValidatorTests()
    {
        _validator = new UserValidator();
        _faker = new Faker();
    }

    [Fact(DisplayName = "Valid user should pass all validation rules")]
    public void Given_ValidUser_When_Validated_Then_ShouldNotHaveErrors()
    {
        var user = GenerateValidUser();

        var result = _validator.TestValidate(user);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "Invalid username formats should fail validation")]
    [InlineData("")]
    [InlineData("ab")]
    public void Given_InvalidUsername_When_Validated_Then_ShouldHaveError(string username)
    {
        var user = GenerateValidUser();
        user.Username = username;

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact(DisplayName = "Username longer than maximum length should fail validation")]
    public void Given_UsernameLongerThanMaximum_When_Validated_Then_ShouldHaveError()
    {
        var user = GenerateValidUser();
        user.Username = _faker.Random.String2(51);

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Theory(DisplayName = "Invalid email formats should fail validation")]
    [InlineData("invalid-email")]
    [InlineData("user@")]
    [InlineData("@domain.com")]
    [InlineData("user@.com")]
    [InlineData("user@domain.")]
    public void Given_InvalidEmail_When_Validated_Then_ShouldHaveError(string email)
    {
        var user = GenerateValidUser();
        user.Email = email;

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact(DisplayName = "Invalid password formats should fail validation")]
    public void Given_InvalidPassword_When_Validated_Then_ShouldHaveError()
    {
        var user = GenerateValidUser();
        user.Password = _faker.Lorem.Word();

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact(DisplayName = "Invalid phone formats should fail validation")]
    public void Given_InvalidPhone_When_Validated_Then_ShouldHaveError()
    {
        var user = GenerateValidUser();
        user.Phone = _faker.Random.AlphaNumeric(5);

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact(DisplayName = "Unknown status should fail validation")]
    public void Given_UnknownStatus_When_Validated_Then_ShouldHaveError()
    {
        var user = GenerateValidUser();
        user.Status = UserStatus.Unknown;

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [Fact(DisplayName = "None role should fail validation")]
    public void Given_NoneRole_When_Validated_Then_ShouldHaveError()
    {
        var user = GenerateValidUser();
        user.Role = UserRole.None;

        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    private static User GenerateValidUser()
    {
        return new Faker<User>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Password, f => f.Internet.Password(8, true, @"\w", "Test@123"))
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
            .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Suspended))
            .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Customer, UserRole.Admin))
            .RuleFor(u => u.Name, f => new NameInfo(f.Name.FirstName(), f.Name.LastName()))
            .RuleFor(u => u.Address, f => new AddressInfo(
                f.Address.City(),
                f.Address.StreetName(),
                f.Random.Int(1, 1000),
                f.Address.ZipCode(),
                new GeolocationInfo()))
            .Generate();
    }
}

