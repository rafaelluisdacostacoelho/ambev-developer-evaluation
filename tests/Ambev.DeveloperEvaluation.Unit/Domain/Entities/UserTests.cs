using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the User entity class.
/// Tests cover status changes and validation scenarios.
/// </summary>
public class UserTests
{
    [Fact]
    public void User_Should_Create_With_Valid_Parameters()
    {
        var name = new Faker<NameInfo>()
            .RuleFor(n => n.Firstname, f => f.Name.FirstName())
            .RuleFor(n => n.Lastname, f => f.Name.LastName())
            .Generate();

        var address = new Faker<AddressInfo>()
            .RuleFor(a => a.City, f => f.Address.City())
            .RuleFor(a => a.Street, f => f.Address.StreetName())
            .RuleFor(a => a.Number, f => f.Random.Int(1, 1000))
            .RuleFor(a => a.Zipcode, f => f.Address.ZipCode())
            .RuleFor(a => a.Geolocation, f => new GeolocationInfo())
            .Generate();

        var user = new Faker<User>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Status, f => f.PickRandom<UserStatus>())
            .RuleFor(u => u.Role, f => f.PickRandom<UserRole>())
            .RuleFor(u => u.Name, f => name)
            .RuleFor(u => u.Address, f => address)
            .Generate();

        user.Username.Should().NotBeNullOrEmpty();
        user.Email.Should().Contain("@");
        user.Password.Should().NotBeNullOrEmpty();
        user.Phone.Should().NotBeNullOrEmpty();
        user.Status.Should().BeOneOf(UserStatus.Active, UserStatus.Inactive, UserStatus.Suspended);
        user.Role.Should().BeOneOf(UserRole.Admin, UserRole.Customer, UserRole.Admin, UserRole.Manager, UserRole.None);
        user.Name.Should().NotBeNull();
        user.Address.Should().NotBeNull();
    }

    [Fact]
    public void User_Should_Activate_Successfully()
    {
        var user = new User { Status = UserStatus.Inactive };
        user.Activate();
        user.Status.Should().Be(UserStatus.Active);
        user.UpdatedAt.Should().NotBeNull();
    }
}