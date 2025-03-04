using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Commands;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users.UpdateUser;

public class UpdateUserHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly UpdateUserHandler _handler;
    private readonly Faker _faker = new("pt_BR");

    public UpdateUserHandlerTests()
    {
        _handler = new UpdateUserHandler(_userRepository, _mapper);
    }

    [Fact]
    public async Task Handle_Should_UpdateUser_With_CompleteCommand()
    {
        var command = GenerateFakeUpdateUserCommand();
        var user = new User { Id = command.Id, Email = command.Email };
        var response = GenerateFakeUpdateUserResponse(command);

        _mapper.Map<User>(command).Returns(user);
        _userRepository.UpdateAsync(user, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<UpdateUserResponse>(user).Returns(response);

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

        await _userRepository.Received(1).UpdateAsync(user, Arg.Any<CancellationToken>());
    }

    private UpdateUserCommand GenerateFakeUpdateUserCommand()
    {
        return new UpdateUserCommand(
            _faker.Internet.UserName(),
            _faker.Internet.Email(),
            _faker.Phone.PhoneNumber(),
            _faker.Internet.Password(),
            _faker.PickRandom<UserRole>(),
            _faker.PickRandom<UserStatus>(),
            new UpdateNameInfoCommand(_faker.Name.FirstName(), _faker.Name.LastName()),
            new UpdateAddressInfoCommand(
                _faker.Address.City(),
                _faker.Address.StreetName(),
                _faker.Random.Int(1, 1000),
                _faker.Address.ZipCode(),
                new UpdateGeolocationInfoCommand
                {
                    Latitude = _faker.Address.Latitude(),
                    Longitude = _faker.Address.Longitude()
                }
            )
        )
        {
            Id = Guid.NewGuid()
        };
    }

    private UpdateUserResponse GenerateFakeUpdateUserResponse(UpdateUserCommand command)
    {
        return new UpdateUserResponse
        {
            Id = command.Id,
            Username = command.Username,
            Password = command.Password,
            Phone = command.Phone,
            Email = command.Email,
            Status = command.Status,
            Role = command.Role,
            Name = new UpdateNameInfoResponse
            {
                Firstname = command.Name.Firstname,
                Lastname = command.Name.Lastname
            },
            Address = new UpdateAddressInfoResponse
            {
                City = command.Address.City,
                Street = command.Address.Street,
                Number = command.Address.Number,
                Zipcode = command.Address.Zipcode,
                Geolocation = new UpdateGeolocationInfoResponse
                {
                    Latitude = command.Address.Geolocation.Latitude,
                    Longitude = command.Address.Geolocation.Longitude
                }
            }
        };
    }
}
