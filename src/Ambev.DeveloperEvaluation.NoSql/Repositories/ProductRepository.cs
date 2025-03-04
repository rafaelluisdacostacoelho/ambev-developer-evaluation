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

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(product, null, cancellationToken);
        return product;
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
        await _collection.ReplaceOneAsync(filter, product, new ReplaceOptions(), cancellationToken);
        return product;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        var result = await _collection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<List<string>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _collection.AsQueryable()
            .Select(p => p.Category.Name)
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginatedResult<Product>> GetProductsByCategoryAsync(string category, int page = 1, int size = 10, string? order = null, CancellationToken cancellationToken = default)
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

        var totalCount = await query.CountAsync(cancellationToken);
        var products = await query.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);

        return new PaginatedResult<Product>(products, totalCount, page, size);
    }
}
