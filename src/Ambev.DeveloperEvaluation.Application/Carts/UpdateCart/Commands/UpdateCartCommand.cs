using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart.Responses;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart.Commands;

/// <summary>
/// Command for creating a new cart.
/// </summary>
/// <remarks>
/// This command captures the necessary data to create a cart, 
/// including the user ID, creation date, and a list of products.
///
/// Implements <see cref="IRequest{TResponse}"/> to initiate a 
/// request that returns a <see cref="UpdateCartResponse"/>.
///
/// The data provided in this command is validated using the 
/// <see cref="UpdateCartCommandValidator"/>, which extends 
/// <see cref="AbstractValidator{T}"/> to ensure the fields are 
/// properly populated and comply with the required rules.
/// </remarks>
public class UpdateCartCommand : IRequest<UpdateCartResponse>
{
    /// <summary>
    /// Cart ID that will be updated.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The unique identifier of the user owner that is updating the cart.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Products with quantity and calculated price (with discounts)
    /// </summary>
    public List<UpdateCartItemCommand> Products { get; private set; } = [];

    public UpdateCartCommand(Guid userId, List<UpdateCartItemCommand> products)
    {
        UserId = userId;
        Products = products;
    }
}
