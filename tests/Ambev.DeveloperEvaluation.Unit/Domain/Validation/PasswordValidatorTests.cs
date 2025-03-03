using Ambev.DeveloperEvaluation.Domain.Validation;
using Bogus;
using FluentAssertions;
using FluentValidation.TestHelper;
using System.Text.RegularExpressions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class PasswordValidatorTests
{
    private readonly PasswordValidator _validator;

    public PasswordValidatorTests()
    {
        _validator = new PasswordValidator();
    }

    [Fact(DisplayName = "Password should match the specified length")]
    public void Given_PasswordLength_When_Validated_Then_ShouldMatchLength()
    {
        var length = 12;
        var password = GeneratePassword(length);

        password.Length.Should().Be(length);
    }

    [Fact(DisplayName = "Memorable password should pass validation")]
    public void Given_MemorablePassword_When_Validated_Then_ShouldNotHaveErrors()
    {
        var password = GeneratePassword(length: 10, regexPattern: @"[A-Za-z0-9\!\?\*\.\@\#\$\%\^\&\+\=]");

        var result = _validator.TestValidate(password);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Password should match the regex pattern")]
    public void Given_PasswordWithRegexPattern_When_Validated_Then_ShouldMatchPattern()
    {
        const string regexPattern = @"[A-Za-z0-9\!\?\*\.\@\#\$\%\^\&\+\=]";
        var password = GeneratePassword(regexPattern: regexPattern);

        Regex.IsMatch(password, regexPattern).Should().BeTrue();
    }

    [Fact(DisplayName = "Password should start with the specified prefix")]
    public void Given_PasswordWithPrefix_When_Validated_Then_ShouldStartWithPrefix()
    {
        const string prefix = "TestPrefix";
        var password = GeneratePassword(prefix: prefix);

        password.Should().StartWith(prefix);
    }

    private static string GeneratePassword(int length = 10, string regexPattern = @"[A-Za-z0-9\!\?\*\.\@\#\$\%\^\&\+\=]", string prefix = "")
    {
        var faker = new Faker();

        // Garante que a senha inclua todos os requisitos mínimos
        string upper = faker.Random.Char('A', 'Z').ToString(); // Letra maiúscula
        string lower = faker.Random.Char('a', 'z').ToString(); // Letra minúscula
        string digit = faker.Random.Char('0', '9').ToString(); // Número
        string special = faker.Random.String2(1, @"!?.@#$%^&*+="); // Caractere especial

        // Gera o restante da senha com o padrão fornecido
        string remaining = faker.Random.String2(Math.Max(0, length - (upper.Length + lower.Length + digit.Length + special.Length + prefix.Length)), regexPattern);

        // Embaralha todos os caracteres para evitar previsibilidade
        return prefix + new string([.. (upper + lower + digit + special + remaining).ToCharArray().OrderBy(_ => faker.Random.Int())]);
    }
}
