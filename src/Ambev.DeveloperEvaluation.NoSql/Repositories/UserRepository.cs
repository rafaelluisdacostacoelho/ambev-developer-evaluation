using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Ambev.DeveloperEvaluation.NoSql.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _collection;

    public UserRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<User>("Users");
    }

    public async Task<PaginatedResult<User>> GetPaginatedAsync(int page = 1, int size = 10, string? order = null, CancellationToken cancellationToken = default)
    {
        var query = _collection.AsQueryable();

        if (!string.IsNullOrEmpty(order))
        {
            query = order switch
            {
                "username_desc" => query.OrderByDescending(p => p.Username),
                "username_asc" => query.OrderBy(p => p.Username),
                "email_asc" => query.OrderBy(p => p.Email),
                "email_desc" => query.OrderByDescending(p => p.Email),
                _ => query
            };
        }

        var totalCount = await query.CountAsync(cancellationToken: cancellationToken);
        var users = await query.Skip((page - 1) * size)
                               .Take(size)
                               .ToListAsync(cancellationToken: cancellationToken);

        return new PaginatedResult<User>(users, totalCount, page, size);
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(user, cancellationToken: cancellationToken);
        return user;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _collection.DeleteOneAsync(u => u.Id == id, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _collection.AsQueryable()
                                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _collection.AsQueryable()
                                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var result = await _collection.ReplaceOneAsync(
            u => u.Id == user.Id,
            user,
            new ReplaceOptions { IsUpsert = false },
            cancellationToken
        );

        return result.MatchedCount > 0 ? user : null;
    }
}
