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
    private readonly Faker _faker = new();

    [Fact]
    public void User_Creation_With_Valid_Data_Should_Succeed()
    {
        // Arrange
        var name = new NameInfo { Firstname = _faker.Name.FirstName(), Lastname = _faker.Name.LastName() };
        var address = new AddressInfo { Street = _faker.Address.StreetAddress(), City = _faker.Address.City(), Zipcode = _faker.Address.ZipCode() };
        var username = _faker.Internet.UserName();
        var email = _faker.Internet.Email();
        var password = _faker.Internet.Password();
        var phone = _faker.Phone.PhoneNumber();

        // Act
        var user = new User(username, email, password, phone, UserStatus.Inactive, UserRole.Customer, name, address);

        // Assert
        user.Username.Should().Be(username);
        user.Email.Should().Be(email);
        user.Password.Should().Be(password);
        user.Phone.Should().Be(phone);
        user.Status.Should().Be(UserStatus.Inactive);
        user.Role.Should().Be(UserRole.Customer);
        user.Name.Should().Be(name);
        user.Address.Should().Be(address);
    }

    [Fact]
    public void User_Creation_With_Empty_Username_Should_Throw_Exception()
    {
        // Arrange
        var name = new NameInfo { Firstname = _faker.Name.FirstName(), Lastname = _faker.Name.LastName() };
        var address = new AddressInfo { Street = _faker.Address.StreetAddress(), City = _faker.Address.City(), Zipcode = _faker.Address.ZipCode() };

        // Act & Assert
        Action act = () => new User("", _faker.Internet.Email(), _faker.Internet.Password(), _faker.Phone.PhoneNumber(), UserStatus.Inactive, UserRole.Customer, name, address);
        act.Should().Throw<ArgumentException>().WithMessage("*Username cannot be empty.*");
    }

    [Fact]
    public void Activate_Should_Set_Status_To_Active()
    {
        // Arrange
        var user = new User { Status = UserStatus.Inactive };

        // Act
        user.Activate();

        // Assert
        user.Status.Should().Be(UserStatus.Active);
        user.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Deactivate_Should_Set_Status_To_Inactive()
    {
        // Arrange
        var user = new User { Status = UserStatus.Active };

        // Act
        user.Deactivate();

        // Assert
        user.Status.Should().Be(UserStatus.Inactive);
        user.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Suspend_Should_Set_Status_To_Suspended()
    {
        // Arrange
        var user = new User { Status = UserStatus.Active };

        // Act
        user.Suspend();

        // Assert
        user.Status.Should().Be(UserStatus.Suspended);
        user.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void UpdateAddressInfo_Should_Update_Address()
    {
        // Arrange
        var user = new User();
        var newAddress = new AddressInfo { Street = _faker.Address.StreetAddress(), City = _faker.Address.City(), Zipcode = _faker.Address.ZipCode() };

        // Act
        user.UpdateAddressInfo(newAddress);

        // Assert
        user.Address.Should().Be(newAddress);
    }

    [Fact]
    public void UpdateNameInfo_Should_Update_Name()
    {
        // Arrange
        var user = new User();
        var newName = new NameInfo { Firstname = _faker.Name.FirstName(), Lastname = _faker.Name.LastName() };

        // Act
        user.UpdateNameInfo(newName);

        // Assert
        user.Name.Should().Be(newName);
    }
}