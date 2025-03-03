using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CategoryInfoTests
{
    [Fact]
    public void CategoryInfo_Should_Create_With_Valid_Parameters()
    {
        var categoryInfo = new Faker<CategoryInfo>()
            .CustomInstantiator(f => new CategoryInfo(f.Random.Guid().ToString(), f.Commerce.Categories(1)[0]))
            .Generate();

        categoryInfo.ExternalId.Should().NotBeNullOrEmpty();
        categoryInfo.Name.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void CategoryInfo_Should_Throw_Exception_With_Empty_ExternalId()
    {
        Action act = static () => _ = new CategoryInfo("", "Valid Name");
        act.Should().Throw<ArgumentException>()
            .WithMessage("ExternalId cannot be empty. (Parameter 'externalId')");
    }

    [Fact]
    public void CategoryInfo_Should_Throw_Exception_With_Empty_Name()
    {
        Action act = () => _ = new CategoryInfo("ValidExternalId", "");
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be empty. (Parameter 'name')");
    }
}
