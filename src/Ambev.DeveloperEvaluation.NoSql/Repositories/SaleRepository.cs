using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.NoSql.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly IMongoCollection<Sale> _collection;

    public SaleRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Sale>("Sales");
    }

    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(sale, null, cancellationToken);
        return sale;
    }

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Sale>.Filter.Eq(p => p.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
}
