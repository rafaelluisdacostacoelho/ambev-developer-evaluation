using Ambev.DeveloperEvaluation.Application.Products.GetProduct.Responses;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct.Commands;

public class GetProductCommand : IRequest<GetProductResponse>
{
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of GetProductCommand
    /// </summary>
    public GetProductCommand() { }

    /// <summary>
    /// Initializes a new instance of GetProductCommand
    /// </summary>
    /// <param name="id">The ID of the product to retrieve</param>
    public GetProductCommand(Guid id)
    {
        Id = id;
    }
}
