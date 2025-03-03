using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using FluentAssertions;
using System.Reflection;
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

    [Fact(DisplayName = "Should create an instance using the private constructor")]
    public void Given_PrivateConstructor_When_Invoked_Then_ShouldCreateInstance()
    {
        var constructorInfo = typeof(CategoryInfo).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null, Type.EmptyTypes, null);

        constructorInfo.Should().NotBeNull("The private constructor should exist.");

        var categoryInfo = (CategoryInfo)constructorInfo!.Invoke(null);

        categoryInfo.Should().NotBeNull();
        categoryInfo.ExternalId.Should().BeEmpty();
        categoryInfo.Name.Should().BeEmpty();
    }
}
