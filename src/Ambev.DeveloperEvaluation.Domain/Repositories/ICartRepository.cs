using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ICartRepository
{
    Task<PaginatedResult<Cart>> GetPaginatedAsync(int page = 1, int size = 10, string? order = null);
    Task<Cart?> GetByIdAsync(Guid id);
    Task AddAsync(Cart cart);
    Task UpdateAsync(Cart cart);
    Task DeleteAsync(Guid id);
}
