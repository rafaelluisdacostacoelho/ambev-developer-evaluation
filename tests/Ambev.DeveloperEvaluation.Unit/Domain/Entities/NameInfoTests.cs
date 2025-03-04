using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class NameInfoTests
{
    [Fact]
    public void NameInfo_Should_Create_With_Valid_Parameters()
    {
        var nameInfo = new Faker<NameInfo>()
            .RuleFor(n => n.Firstname, f => f.Name.FirstName())
            .RuleFor(n => n.Lastname, f => f.Name.LastName())
            .Generate();

        nameInfo.Firstname.Should().NotBeNullOrEmpty();
        nameInfo.Lastname.Should().NotBeNullOrEmpty();
    }
}
