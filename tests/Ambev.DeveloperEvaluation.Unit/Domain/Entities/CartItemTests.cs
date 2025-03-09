using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using System.Reflection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CartItemTests
{
    [Fact(DisplayName = "Should create CartItem with valid parameters")]
    public void Given_ValidParameters_When_CreatingCartItem_Then_ShouldCreateSuccessfully()
    {
        // Arrange
        var productId = Guid.NewGuid();
        const int quantity = 10;

        // Act
        var cartItem = new CartItem(productId, quantity, 5);

        // Assert
        cartItem.ProductId.Should().Be(productId);
        cartItem.Quantity.Should().Be(quantity);
    }

    [Theory(DisplayName = "Should throw exception when quantity is out of range")]
    [InlineData(0)]
    [InlineData(21)]
    public void Given_InvalidQuantity_When_CreatingCartItem_Then_ShouldThrowException(int quantity)
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        Action act = () => new CartItem(productId, quantity, 5);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithMessage("Quantity must be between 1 and 20*");
    }

    [Fact(DisplayName = "Should increase quantity successfully")]
    public void Given_ValidAmount_When_IncreasingQuantity_Then_ShouldIncreaseSuccessfully()
    {
        // Arrange
        var cartItem = new CartItem(Guid.NewGuid(), 10, 5);

        // Act
        cartItem.UpdateProductQuantity(15, 10);

        // Assert
        cartItem.Quantity.Should().Be(15);
    }

    [Fact(DisplayName = "Should throw exception when increasing quantity exceeds maximum")]
    public void Given_InvalidAmount_When_IncreasingQuantity_Then_ShouldThrowException()
    {
        // Arrange
        var cartItem = new CartItem(Guid.NewGuid(), 18, 5);

        // Act
        Action act = () => cartItem.UpdateProductQuantity(23, 10);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithMessage("Quantity must be between 1 and 20*");
    }

    [Fact(DisplayName = "Should decrease quantity successfully")]
    public void Given_ValidAmount_When_DecreasingQuantity_Then_ShouldDecreaseSuccessfully()
    {
        // Arrange
        var cartItem = new CartItem(Guid.NewGuid(), 10, 5);

        // Act
        cartItem.UpdateProductQuantity(5, 10);

        // Assert
        cartItem.Quantity.Should().Be(5);
    }

    [Fact(DisplayName = "Should throw exception when decreasing quantity below minimum")]
    public void Given_InvalidAmount_When_DecreasingQuantity_Then_ShouldThrowException()
    {
        // Arrange
        var cartItem = new CartItem(Guid.NewGuid(), 5, 5);

        // Act
        Action act = () => cartItem.UpdateProductQuantity(0, 10);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithMessage("Quantity must be between 1 and 20*");
    }

    [Fact(DisplayName = "Should create instance using private constructor")]
    public void Given_PrivateConstructor_When_Invoked_Then_ShouldCreateInstance()
    {
        // Usa reflexão para acessar o construtor privado
        var constructorInfo = typeof(CartItem).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null, Type.EmptyTypes, null);

        constructorInfo.Should().NotBeNull("The private constructor should exist.");

        // Invoca o construtor privado para criar uma instância
        var cartItem = (CartItem)constructorInfo!.Invoke(null);

        cartItem.Should().NotBeNull();
        cartItem.Quantity.Should().Be(0);
        cartItem.ProductId.Should().Be(Guid.Empty);
    }
}
