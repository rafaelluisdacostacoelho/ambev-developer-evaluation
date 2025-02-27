using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IProductRepository
{
    Task<PaginatedResult<Product>> GetPaginatedAsync(int page = 1, int size = 10, string? order = null);
    Task<Product?> GetByIdAsync(Guid id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid id);
    Task<List<string>> GetCategoriesAsync();
    Task<PaginatedResult<Product>> GetProductsByCategoryAsync(string category, int page = 1, int size = 10, string? order = null);
}
