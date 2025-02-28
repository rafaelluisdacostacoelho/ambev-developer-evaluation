using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.NoSql.Repositories;

public class CartRepository : ICartRepository
{
    public Task AddAsync(Cart cart)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Cart?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedResult<Cart>> GetPaginatedAsync(int page = 1, int size = 10, string? order = null)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Cart cart)
    {
        throw new NotImplementedException();
    }
}
