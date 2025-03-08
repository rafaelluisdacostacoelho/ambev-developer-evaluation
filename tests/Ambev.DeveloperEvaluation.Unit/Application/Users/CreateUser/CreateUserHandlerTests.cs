using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser.Commands;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser.Responses;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users.CreateUsers;

/// <summary>
/// Contains unit tests for the <see cref="CreateUserHandler"/> class.
/// </summary>
public class CreateUserHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly CreateUserHandler _handler;
    private readonly Faker _faker = new Faker("pt_BR");

    public CreateUserHandlerTests()
    {
        _handler = new CreateUserHandler(_userRepository, _mapper, _passwordHasher);
    }

    [Fact]
    public async Task Handle_Should_CreateUser_With_CompleteCommand()
    {
        var command = GenerateFakeCreateUserCommand();
        var user = new User { Email = command.Email, Password = "hashedPassword" };
        var response = GenerateFakeCreateUserResponse(command);

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns((User?)null);
        _mapper.Map<User>(command).Returns(user);
        _passwordHasher.HashPassword(command.Password).Returns("hashedPassword");
        _userRepository.CreateAsync(user, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<CreateUserResponse>(user).Returns(response);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(response.Id);
        result.Username.Should().Be(response.Username);
        result.Password.Should().Be(response.Password);
        result.Phone.Should().Be(response.Phone);
        result.Status.Should().Be(response.Status);
        result.Role.Should().Be(response.Role);
        result.Name.Firstname.Should().Be(response.Name.Firstname);
        result.Name.Lastname.Should().Be(response.Name.Lastname);
        result.Address.City.Should().Be(response.Address.City);
        result.Address.Street.Should().Be(response.Address.Street);
        result.Address.Number.Should().Be(response.Address.Number);
        result.Address.Zipcode.Should().Be(response.Address.Zipcode);
        result.Address.Geolocation.Latitude.Should().Be(response.Address.Geolocation.Latitude);
        result.Address.Geolocation.Longitude.Should().Be(response.Address.Geolocation.Longitude);
        await _userRepository.Received(1).CreateAsync(user, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_UserAlreadyExists()
    {
        var command = GenerateFakeCreateUserCommand();
        var existingUser = new User { Email = command.Email };

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(existingUser);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"User with email {command.Email} already exists");
    }

    [Fact]
    public async Task Handle_Should_HashPassword_When_CreatingUser()
    {
        var command = GenerateFakeCreateUserCommand();
        var user = new User { Email = command.Email, Password = "" };

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns((User?)null);
        _mapper.Map<User>(command).Returns(user);
        _passwordHasher.HashPassword(command.Password).Returns("hashedPassword");

        await _handler.Handle(command, CancellationToken.None);

        user.Password.Should().Be("hashedPassword");
        _passwordHasher.Received(1).HashPassword(command.Password);
    }

    [Fact]
    public async Task Handle_Should_CallRepositoryMethods_WithExpectedParameters()
    {
        var command = GenerateFakeCreateUserCommand();
        var user = new User { Email = command.Email };

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns((User?)null);
        _mapper.Map<User>(command).Returns(user);

        await _handler.Handle(command, CancellationToken.None);

        await _userRepository.Received(1).GetByEmailAsync(command.Email, Arg.Any<CancellationToken>());
        await _userRepository.Received(1).CreateAsync(user, Arg.Any<CancellationToken>());
    }

    private CreateUserResponse GenerateFakeCreateUserResponse(CreateUserCommand command)
    {
        return new CreateUserResponse
        {
            Id = Guid.NewGuid(),
            Username = command.Username,
            Password = command.Password,
            Phone = command.Phone,
            Email = command.Email,
            Status = command.Status,
            Role = command.Role,
            Name = new CreateNameInfoResponse
            {
                Firstname = command.Name.Firstname,
                Lastname = command.Name.Lastname
            },
            Address = new CreateAddressInfoResponse
            {
                City = command.Address.City,
                Street = command.Address.Street,
                Number = command.Address.Number,
                Zipcode = command.Address.Zipcode,
                Geolocation = new CreateGeolocationInfoResponse
                {
                    Latitude = command.Address.Geolocation.Latitude,
                    Longitude = command.Address.Geolocation.Longitude
                }
            }
        };
    }

    private CreateUserCommand GenerateFakeCreateUserCommand()
    {
        return new CreateUserCommand
        {
            Username = _faker.Internet.UserName(),
            Password = _faker.Internet.Password(),
            Phone = _faker.Phone.PhoneNumber(),
            Email = _faker.Internet.Email(),
            Status = _faker.PickRandom<UserStatus>(),
            Role = _faker.PickRandom<UserRole>(),
            Name = new CreateNameInfoCommand { Firstname = _faker.Name.FirstName(), Lastname = _faker.Name.LastName() },
            Address = new CreateAddressInfoCommand
            {
                City = _faker.Address.City(),
                Street = _faker.Address.StreetName(),
                Number = _faker.Random.Int(1, 1000),
                Zipcode = _faker.Address.ZipCode(),
                Geolocation = new CreateGeolocationInfoCommand
                {
                    Latitude = _faker.Address.Latitude(),
                    Longitude = _faker.Address.Longitude()
                }
            }
        };
    }
}
