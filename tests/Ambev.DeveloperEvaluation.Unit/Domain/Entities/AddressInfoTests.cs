using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class AddressInfoTests
{
    [Fact]
    public void AddressInfo_Should_Create_With_Valid_Parameters()
    {
        var addressInfo = new Faker<AddressInfo>()
            .RuleFor(a => a.City, f => f.Address.City())
            .RuleFor(a => a.Street, f => f.Address.StreetName())
            .RuleFor(a => a.Number, f => f.Random.Int(1, 1000))
            .RuleFor(a => a.Zipcode, f => f.Address.ZipCode())
            .RuleFor(a => a.Geolocation, f => new GeolocationInfo())
            .Generate();

        addressInfo.City.Should().NotBeNullOrEmpty();
        addressInfo.Street.Should().NotBeNullOrEmpty();
        addressInfo.Number.Should().BeGreaterThan(0);
        addressInfo.Zipcode.Should().NotBeNullOrEmpty();
        addressInfo.Geolocation.Should().NotBeNull();
    }
}
