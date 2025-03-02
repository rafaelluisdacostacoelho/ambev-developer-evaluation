using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct.Responses;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct.Commands;

public class DeleteProductCommand : IRequest<DeleteProductResponse>
{
    public Guid Id { get; set; }

    public DeleteProductCommand(Guid id)
    {
        Id = id;
    }
}
