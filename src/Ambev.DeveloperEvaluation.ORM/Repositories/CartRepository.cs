using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class CartRepository : ICartRepository
{
    private readonly StoreDbContext _context;

    /// <summary>
    /// Initializes a new instance of CartRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public CartRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<Cart>> GetPaginatedAsync(int page = 1, int size = 10, string? order = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Carts
                            .Include(c => c.Products)
                            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(order))
        {
            query = ApplySorting(query, order);
        }

        var totalItems = await query.CountAsync(cancellationToken: cancellationToken);

        var items = await query.Skip((page - 1) * size)
                               .Take(size)
                               .ToListAsync(cancellationToken: cancellationToken);

        return new PaginatedResult<Cart>(items, totalItems, page, size);
    }

    public async Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Carts
                             .Include(c => c.Products)
                             .FirstOrDefaultAsync(c => c.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        await _context.Carts.AddAsync(cart, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return cart;
    }

    public async Task<Cart> UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        var existingCart = await _context.Carts
                                         .Include(c => c.Products)
                                         .AsNoTracking()
                                         .SingleOrDefaultAsync(c => c.Id == cart.Id, cancellationToken: cancellationToken)
            ?? throw new KeyNotFoundException("Carrinho não encontrado.");

        // Limpar os itens existentes e adicionar os novos
        existingCart.Products.Clear();
        existingCart.Products.AddRange(cart.Products);

        _context.Entry(existingCart).CurrentValues.SetValues(cart);

        await _context.SaveChangesAsync(cancellationToken);

        return cart;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cart = await _context.Carts.FindAsync([id], cancellationToken: cancellationToken)
            ?? throw new KeyNotFoundException("Carrinho não encontrado.");

        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Aplica ordenação dinâmica a uma query usando uma string de ordenação no formato "campo1 asc, campo2 desc"
    /// </summary>
    private static IQueryable<Cart> ApplySorting(IQueryable<Cart> query, string order)
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
