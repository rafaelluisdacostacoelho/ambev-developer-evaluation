using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;

public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, PaginatedResult<Product>>
{
    private readonly IProductRepository _repository;

    public GetProductsByCategoryQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedResult<Product>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetProductsByCategoryAsync(request.Category, request.Page, request.Size, request.Order, cancellationToken);
    }
}
