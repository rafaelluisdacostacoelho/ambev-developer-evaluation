using Ambev.DeveloperEvaluation.Application.Pagination;
using Ambev.DeveloperEvaluation.Application.Users.ListUsers.Responses;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUsers;

public class ListUsersQueryHandler : IRequestHandler<PaginationQuery<ListUsersQuery, ListUserResponse>, PaginatedResponse<ListUserResponse>>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public ListUsersQueryHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<ListUserResponse>> Handle(PaginationQuery<ListUsersQuery, ListUserResponse> request, CancellationToken cancellationToken)
    {
        var paginatedResult = await _repository.GetPaginatedAsync(
            request.PageNumber,
            request.PageSize,
            request.Order,
            cancellationToken: cancellationToken
        );

        var mappedUsers = _mapper.Map<ICollection<ListUserResponse>>(paginatedResult.Items);

        return new PaginatedResponse<ListUserResponse>
        {
            Data = mappedUsers,
            CurrentPage = paginatedResult.CurrentPage,
            TotalPages = paginatedResult.TotalPages,
            TotalCount = paginatedResult.TotalItems
        };
    }
}
