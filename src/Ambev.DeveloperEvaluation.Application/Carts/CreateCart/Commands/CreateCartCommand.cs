using Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Responses;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Validators;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Commands;

/// <summary>
/// Command for creating a new cart.
/// </summary>
/// <remarks>
/// This command captures the necessary data to create a cart, 
/// including the user ID, creation date, and a list of products.
///
/// Implements <see cref="IRequest{TResponse}"/> to initiate a 
/// request that returns a <see cref="CreateCartResponse"/>.
///
/// The data provided in this command is validated using the 
/// <see cref="CreateCartCommandValidator"/>, which extends 
/// <see cref="AbstractValidator{T}"/> to ensure the fields are 
/// properly populated and comply with the required rules.
/// </remarks>
public class CreateCartCommand : IRequest<CreateCartResponse>
{
    /// <summary>
    /// The unique identifier of the user creating the cart.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// The date when the cart was created.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// The list of products to be added to the cart.
    /// Each item in the list represents a product with specific details and quantity.
    /// </summary>
    public List<CreateCartItemCommand> Products { get; private set; } = [];

    public CreateCartCommand(Guid userId, DateTime date, List<CreateCartItemCommand> products)
    {
        UserId = userId;
        Date = date;
        Products = products;
    }
}
