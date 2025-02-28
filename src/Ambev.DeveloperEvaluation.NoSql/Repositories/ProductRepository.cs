using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.NoSql.Repositories;

public class ProductRepository : IProductRepository
{
    public Task AddAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Product?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetCategoriesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedResult<Product>> GetPaginatedAsync(int page = 1, int size = 10, string? order = null)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedResult<Product>> GetProductsByCategoryAsync(string category, int page = 1, int size = 10, string? order = null)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Product product)
    {
        throw new NotImplementedException();
    }
}
