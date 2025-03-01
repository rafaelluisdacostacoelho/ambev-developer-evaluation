using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Pagination;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public class GetProductsQuery : IRequest<PaginatedResult<Product>>
{
    public int Page { get; }
    public int Size { get; }
    public string? Order { get; }

    public GetProductsQuery(int page = 1, int size = 10, string? order = null)
    {
        Page = page;
        Size = size;
        Order = order;
    }
}
