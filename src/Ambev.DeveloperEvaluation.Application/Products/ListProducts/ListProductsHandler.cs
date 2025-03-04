using Ambev.DeveloperEvaluation.Application.Pagination;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts.Responses;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsHandler : IRequestHandler<PaginationQuery<ListProductsQuery, ListProductResponse>, PaginatedResponse<ListProductResponse>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ListProductsHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<ListProductResponse>> Handle(PaginationQuery<ListProductsQuery, ListProductResponse> request, CancellationToken cancellationToken)
    {
        var paginatedResult = await _repository.GetPaginatedAsync(
            request.PageNumber,
            request.PageSize,
            request.Order,
            cancellationToken: cancellationToken
        );

        var mappedProducts = _mapper.Map<ICollection<ListProductResponse>>(paginatedResult.Items);

        return new PaginatedResponse<ListProductResponse>
        {
            Data = mappedProducts,
            CurrentPage = paginatedResult.CurrentPage,
            TotalPages = paginatedResult.TotalPages,
            TotalCount = paginatedResult.TotalItems
        };
    }
}
