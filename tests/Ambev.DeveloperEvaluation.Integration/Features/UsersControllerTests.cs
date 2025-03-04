using Ambev.DeveloperEvaluation.Application.Users.CreateUser.Commands;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser.Responses;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Integration.Common;
using Bogus;
using FluentAssertions;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Features;

public class UsersControllerTests : IntegrationTestBase, IClassFixture<AmbevWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UsersControllerTests(AmbevWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        InitializeAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnCreated_WhenUserIsValid()
    {
        // Arrange
        var faker = new Faker("pt_BR");
        var command = new CreateUserCommand
        {
            Username = faker.Internet.UserName(),
            Password = faker.Internet.Password(),
            Phone = faker.Phone.PhoneNumber("+55 (##) #####-####"),
            Email = faker.Internet.Email(),
            Status = faker.PickRandom<UserStatus>(),
            Role = faker.PickRandom<UserRole>(),
            Name = new CreateNameInfoCommand
            {
                Firstname = faker.Name.FirstName(),
                Lastname = faker.Name.LastName()
            },
            Address = new CreateAddressInfoCommand
            {
                City = faker.Address.City(),
                Street = faker.Address.StreetName(),
                Number = faker.Random.Int(1, 1000),
                Zipcode = faker.Address.ZipCode("#####-###"),
                Geolocation = new CreateGeolocationInfoCommand
                {
                    Latitude = faker.Address.Latitude(),
                    Longitude = faker.Address.Longitude()
                }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<CreateUserResponse>();
        result.Should().NotBeNull();
        result.Username.Should().Be(command.Username);
        result.Name.Firstname.Should().Be(command.Name.Firstname);
        result.Name.Lastname.Should().Be(command.Name.Lastname);
        result.Address.City.Should().Be(command.Address.City);
        result.Address.Street.Should().Be(command.Address.Street);
        result.Address.Number.Should().Be(command.Address.Number);
        result.Address.Zipcode.Should().Be(command.Address.Zipcode);
        result.Address.Geolocation.Latitude.Should().Be(command.Address.Geolocation.Latitude);
        result.Address.Geolocation.Longitude.Should().Be(command.Address.Geolocation.Longitude);
    }
}