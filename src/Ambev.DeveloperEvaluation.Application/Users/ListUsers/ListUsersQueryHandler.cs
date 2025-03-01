using Ambev.DeveloperEvaluation.Application.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUsers;

public class ListUsersQueryHandler : IRequestHandler<PaginationQuery<ListUsersFilter, ListUsersResponse>, PaginatedResponse<ListUsersResponse>>
{
    private readonly IUserRepository _repository;

    public ListUsersQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedResponse<ListUsersResponse>> Handle(PaginationQuery<ListUsersFilter, ListUsersResponse> request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetPaginatedAsync(request.PageNumber, request.PageSize, request.Order, cancellationToken: cancellationToken);

        return new PaginatedResponse<ListUsersResponse>
        {
            Data = result.Items.Select(u => new ListUsersResponse()),
            CurrentPage = result.CurrentPage,
            TotalPages = result.TotalPages,
            TotalCount = result.TotalItems,
            Success = true
        };
    }
}
