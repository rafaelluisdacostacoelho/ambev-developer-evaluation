using Ambev.DeveloperEvaluation.Domain.Entities;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.NoSql;

public class StoreNoSqlContext
{
    private readonly IMongoDatabase _database;

    public StoreNoSqlContext(IMongoDatabase database)
    {
        _database = database ?? throw new ArgumentNullException(nameof(database), "O banco de dados MongoDB não pode ser nulo.");
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
    public IMongoCollection<Cart> Carts => _database.GetCollection<Cart>("Carts");
}
