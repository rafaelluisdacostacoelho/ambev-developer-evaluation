using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart.Responses;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart.Commands;

/// <summary>
/// Command for deleting a cart
/// </summary>
public record DeleteCartCommand : IRequest<DeleteCartResponse>
{
    /// <summary>
    /// The unique identifier of the cart to delete
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of DeleteCartCommand
    /// </summary>
    /// <param name="id">The ID of the cart to delete</param>
    public DeleteCartCommand(Guid id)
    {
        Id = id;
    }
}
