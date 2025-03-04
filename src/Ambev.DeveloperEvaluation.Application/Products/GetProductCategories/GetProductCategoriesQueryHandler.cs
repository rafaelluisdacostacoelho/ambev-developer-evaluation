using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductCategories;

public class GetProductCategoriesQueryHandler : IRequestHandler<GetProductCategoriesQuery, List<string>>
{
    private readonly IProductRepository _repository;

    public GetProductCategoriesQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<string>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetCategoriesAsync(cancellationToken);
    }
}
