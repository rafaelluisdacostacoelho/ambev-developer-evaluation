using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Ambev.DeveloperEvaluation.NoSql.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _collection;

    public ProductRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Product>("Products");
    }

    public async Task<PaginatedResult<Product>> GetPaginatedAsync(int page = 1, int size = 10, string? order = null, CancellationToken cancellationToken = default)
    {
        var query = _collection.AsQueryable();

        if (!string.IsNullOrEmpty(order))
        {
            query = order switch
            {
                "price_desc" => query.OrderByDescending(p => p.Price),
                "price_asc" => query.OrderBy(p => p.Price),
                "title_asc" => query.OrderBy(p => p.Title),
                "title_desc" => query.OrderByDescending(p => p.Title),
                _ => query
            };
        }

        var totalCount = await query.CountAsync(cancellationToken: cancellationToken);
        var products = await query.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken: cancellationToken);

        return new PaginatedResult<Product>(products, totalCount, page, size);
    }

    public async Task AddAsync(Product product)
    {
        await _collection.InsertOneAsync(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        await _collection.DeleteOneAsync(filter);
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        return await _collection.AsQueryable()
            .Select(p => p.Category.Name)
            .Distinct()
            .ToListAsync();
    }

    public async Task<PaginatedResult<Product>> GetProductsByCategoryAsync(string category, int page = 1, int size = 10, string? order = null)
    {
        var query = _collection.AsQueryable()
            .Where(p => p.Category.Name == category);

        if (!string.IsNullOrEmpty(order))
        {
            query = order switch
            {
                "price_desc" => query.OrderByDescending(p => p.Price),
                "price_asc" => query.OrderBy(p => p.Price),
                "title_asc" => query.OrderBy(p => p.Title),
                "title_desc" => query.OrderByDescending(p => p.Title),
                _ => query
            };
        }

        var totalCount = await query.CountAsync();
        var products = await query.Skip((page - 1) * size).Take(size).ToListAsync();

        return new PaginatedResult<Product>(products, totalCount, page, size);
    }

    public async Task UpdateAsync(Product product)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
        await _collection.ReplaceOneAsync(filter, product);
    }
}
