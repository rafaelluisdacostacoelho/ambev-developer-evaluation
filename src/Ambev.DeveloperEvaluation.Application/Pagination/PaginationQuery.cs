using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Pagination;

public class PaginationQuery<TFilter, TResponse> : IRequest<PaginatedResponse<TResponse>>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public string? Order { get; }
    public TFilter? Filter { get; }

    public PaginationQuery() { }

    public PaginationQuery(int pageNumber, int pageSize, string? order, TFilter? filter)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Order = order;
        Filter = filter;
    }
}

public class PaginationQuery<TResponse> : IRequest<PaginatedResponse<TResponse>>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public string? Order { get; }

    public PaginationQuery() { }

    public PaginationQuery(int pageNumber, int pageSize, string? order)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Order = order;
    }
}