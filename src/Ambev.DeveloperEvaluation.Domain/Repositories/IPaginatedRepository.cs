using Ambev.DeveloperEvaluation.Domain.Pagination;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IPaginatedRepository<T> where T : class
{
    Task<PaginatedResult<T>> GetPaginatedAsync(int page = 1, int size = 10, string? order = null, CancellationToken cancellationToken = default);
}
