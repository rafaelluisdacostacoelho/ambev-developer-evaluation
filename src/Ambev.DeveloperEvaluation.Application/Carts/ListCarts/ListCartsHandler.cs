using Ambev.DeveloperEvaluation.Application.Carts.ListCarts.Responses;
using Ambev.DeveloperEvaluation.Application.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

public class ListCartsHandler : IRequestHandler<PaginationQuery<ListCartResponse>, PaginatedResponse<ListCartResponse>>
{
    private readonly ICartRepository _repository;
    private readonly IMapper _mapper;

    public ListCartsHandler(ICartRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<ListCartResponse>> Handle(PaginationQuery<ListCartResponse> request, CancellationToken cancellationToken)
    {
        var paginatedResult = await _repository.GetPaginatedAsync(
            request.PageNumber,
            request.PageSize,
            request.Order,
            cancellationToken: cancellationToken
        );

        var mappedCarts = _mapper.Map<ICollection<ListCartResponse>>(paginatedResult.Items);

        return new PaginatedResponse<ListCartResponse>
        {
            Data = mappedCarts,
            CurrentPage = paginatedResult.CurrentPage,
            TotalPages = paginatedResult.TotalPages,
            TotalCount = paginatedResult.TotalItems
        };
    }
}
