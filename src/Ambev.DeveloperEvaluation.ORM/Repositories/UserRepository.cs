using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IUserRepository using Entity Framework Core (PostgreSQL)
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly StoreDbContext _context;

    public UserRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<User>> GetPaginatedAsync(int page = 1, int size = 10, string? order = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(order))
        {
            query = ApplySorting(query, order);
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query.Skip((page - 1) * size)
                               .Take(size)
                               .ToListAsync(cancellationToken);

        return new PaginatedResult<User>(items, totalItems, page, size);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User?> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var existingUser = await _context.Users.FindAsync([user.Id], cancellationToken);
        if (existingUser == null) return null;

        _context.Entry(existingUser).CurrentValues.SetValues(user);

        existingUser.UpdatedAt = DateTime.UtcNow;

        // Atualiza propriedades complexas manualmente
        existingUser.UpdateAddressInfo(new AddressInfo(user.Address.City, user.Address.Street, user.Address.Number, user.Address.Zipcode, user.Address.Geolocation));
        existingUser.UpdateNameInfo(new NameInfo(user.Name.Firstname, user.Name.Lastname));

        await _context.SaveChangesAsync(cancellationToken);

        return existingUser;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Users.FindAsync([id], cancellationToken);
        if (entity == null)
            return false;

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static IQueryable<User> ApplySorting(IQueryable<User> query, string order)
    {
        var orders = order.Split(',');
        foreach (var orderBy in orders)
        {
            var parts = orderBy.Trim().Split(' ');
            if (parts.Length == 2)
            {
                var property = parts[0];
                var direction = parts[1].Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? "descending" : "ascending";
                query = query.OrderBy($"{property} {direction}");
            }
        }
        return query;
    }
}
