using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation for ActiveUserSpecification tests
/// to ensure consistency across test cases.
/// </summary>
public static class ActiveUserSpecificationTestData
{
    /// <summary>
    /// Configures the Faker to generate valid User entities.
    /// The generated users will have valid:
    /// - Email (valid format)
    /// - Password (meeting complexity requirements)
    /// - FirstName
    /// - LastName
    /// - Phone (Brazilian format)
    /// - Role (User)
    /// Status is not set here as it's the main test parameter
    /// </summary>
    private static readonly Faker<User> userFaker = new Faker<User>()
        .CustomInstantiator(f => new User(username: f.Internet.UserName(),
                                          email: f.Internet.Email(),
                                          password: $"Test@{f.Random.Number(1000)}", // Simulando senha aleatória
                                          phone: "",
                                          status: UserStatus.Active,
                                          role: f.PickRandom<UserRole>(),
                                          name: new NameInfo(f.Name.FirstName(), f.Name.LastName()),
                                          address: new AddressInfo(
                                              city: f.Address.City(),
                                              street: f.Address.StreetName(),
                                              number: f.Random.Int(1, 1000),
                                              zipcode: f.Address.ZipCode(),
                                              geolocation: new GeolocationInfo(
                                                  latitude: f.Address.Latitude(),
                                                  longitude: f.Address.Longitude()
                                              )
                                          )));

    /// <summary>
    /// Generates a valid User entity with the specified status.
    /// </summary>
    /// <param name="status">The UserStatus to set for the generated user.</param>
    /// <returns>A valid User entity with randomly generated data and specified status.</returns>
    public static User GenerateUser(UserStatus status)
    {
        var user = userFaker.Generate();
        user.Status = status;
        return user;
    }
}
