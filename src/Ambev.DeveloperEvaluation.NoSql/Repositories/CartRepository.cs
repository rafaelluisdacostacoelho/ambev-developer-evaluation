using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Ambev.DeveloperEvaluation.NoSql.Repositories;

public class CartRepository : ICartRepository
{
    private readonly IMongoCollection<Cart> _collection;

    public CartRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Cart>("Carts");
    }

    public async Task<PaginatedResult<Cart>> GetPaginatedAsync(int page = 1, int size = 10, string? order = null, CancellationToken cancellationToken = default)
    {
        var query = _collection.AsQueryable();

        if (!string.IsNullOrEmpty(order))
        {
            query = order switch
            {
                "id_desc" => query.OrderByDescending(c => c.Id),
                "id_asc" => query.OrderBy(p => p.Id),
                _ => query
            };
        }

        var totalCount = await query.CountAsync(cancellationToken: cancellationToken);
        var carts = await query.Skip((page - 1) * size)
                               .Take(size)
                               .ToListAsync(cancellationToken: cancellationToken);

        return new PaginatedResult<Cart>(carts, totalCount, page, size);
    }

    public async Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(cart, cancellationToken: cancellationToken);
        return cart;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Cart>.Filter.Eq(c => c.Id, id);
        await _collection.DeleteOneAsync(filter, cancellationToken);
        return true;
    }

    public async Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Cart>.Filter.Eq(c => c.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<Cart> UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Cart>.Filter.Eq(c => c.Id, cart.Id);
        await _collection.ReplaceOneAsync(filter, cart, cancellationToken: cancellationToken);

        return cart;
    }
}
