using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class CartRepository : ICartRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of CartRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public CartRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<Cart>> GetPaginatedAsync(int page = 1, int size = 10, string? order = null)
    {
        var query = _context.Carts
            .Include(c => c.Products) // Carregar os produtos do carrinho
            .AsQueryable();

        // Aplicar ordenação dinâmica
        if (!string.IsNullOrWhiteSpace(order))
        {
            query = ApplySorting(query, order);
        }

        // Contagem total de itens antes da paginação
        var totalItems = await query.CountAsync();

        // Aplicar paginação
        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new PaginatedResult<Cart>(items, totalItems, page, size);
    }

    public async Task<Cart?> GetByIdAsync(Guid id)
    {
        return await _context.Carts
            .Include(c => c.Products) // Carregar os produtos do carrinho
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cart cart)
    {
        var existingCart = await _context.Carts
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == cart.Id) ?? throw new KeyNotFoundException("Carrinho não encontrado.");
        _context.Entry(existingCart).CurrentValues.SetValues(cart);

        // Atualizar produtos do carrinho
        existingCart.Products.Clear();
        existingCart.Products.AddRange(cart.Products);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var cart = await _context.Carts.FindAsync(id);
        if (cart == null)
        {
            throw new KeyNotFoundException("Carrinho não encontrado.");
        }

        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
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
