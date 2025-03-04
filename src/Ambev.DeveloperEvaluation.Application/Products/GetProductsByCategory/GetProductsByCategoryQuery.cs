using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Pagination;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;

public class GetProductsByCategoryQuery : IRequest<PaginatedResult<Product>>
{
    public string Category { get; }
    public int Page { get; }
    public int Size { get; }
    public string? Order { get; }

    public GetProductsByCategoryQuery(string category, int page = 1, int size = 10, string? order = null)
    {
        Category = category;
        Page = page;
        Size = size;
        Order = order;
    }
}
