using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IUserRepository using Entity Framework Core
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly StoreDbContext _context;

    /// <summary>
    /// Initializes a new instance of UserRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public UserRepository(StoreDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a paginated list of users
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="size">Number of items per page (default: 10)</param>
    /// <param name="order">Ordering of results (e.g., "username asc, email desc")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated list of users</returns>
    public async Task<PaginatedResult<User>> GetPaginatedAsync(int page = 1, int size = 10, string? order = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Users.AsQueryable();

        // Aplicar ordenação dinâmica
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

    /// <summary>
    /// Retrieves a user by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the user</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user if found, null otherwise</returns>
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
                             .Include(u => u.Name)
                             .Include(u => u.Address)
                             .ThenInclude(a => a.Geolocation)
                             .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a user by their email address
    /// </summary>
    /// <param name="email">The email address to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user if found, null otherwise</returns>
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
                             .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    /// <summary>
    /// Creates a new user in the database
    /// </summary>
    /// <param name="user">The user to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user</returns>
    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    /// <summary>
    /// Updates an existing user
    /// </summary>
    public async Task<User?> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var existingUser = await _context.Users
                                         .Include(u => u.Name)
                                         .Include(u => u.Address)
                                         .ThenInclude(a => a.Geolocation)
                                         .FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);

        if (existingUser == null)
        {
            return null;
        }

        _context.Entry(existingUser).CurrentValues.SetValues(user);

        // Atualizar informações aninhadas (Nome, Endereço, Geolocalização)
        existingUser.Name = user.Name;
        existingUser.Address = user.Address;
        existingUser.Address.Geolocation = user.Address.Geolocation;

        await _context.SaveChangesAsync(cancellationToken);
        return existingUser;
    }

    /// <summary>
    /// Deletes a user from the database
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the user was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Aplica ordenação dinâmica a uma query usando uma string de ordenação no formato "campo1 asc, campo2 desc"
    /// </summary>
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
