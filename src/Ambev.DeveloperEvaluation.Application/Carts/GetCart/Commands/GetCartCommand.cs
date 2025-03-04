using Ambev.DeveloperEvaluation.Application.Carts.GetCart.Responses;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart.Commands;

/// <summary>
/// Command for retrieving a cart by their ID
/// </summary>
public record GetCartCommand : IRequest<GetCartResponse>
{
    /// <summary>
    /// The unique identifier of the cart to retrieve
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of GetCartCommand
    /// </summary>
    /// <param name="id">The ID of the cart to retrieve</param>
    public GetCartCommand(Guid id)
    {
        Id = id;
    }
}
