using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly StoreDbContext _context;

    /// <summary>
    /// Initializes a new instance of ProductRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public ProductRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<Product>> GetPaginatedAsync(int page = 1, int size = 10, string? order = null)
    {
        var query = _context.Products.AsQueryable();

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

        return new PaginatedResult<Product>
        {
            Data = items,
            TotalItems = totalItems,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalItems / (double)size)
        };
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        var existingProduct = await _context.Products.FindAsync(product.Id) ?? throw new KeyNotFoundException("Produto não encontrado.");
        _context.Entry(existingProduct).CurrentValues.SetValues(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id) ?? throw new KeyNotFoundException("Produto não encontrado.");
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        return await _context.Products
                             .Select(p => p.Category.Name)
                             .Distinct()
                             .ToListAsync();
    }

    public async Task<PaginatedResult<Product>> GetProductsByCategoryAsync(string category, int page = 1, int size = 10, string? order = null)
    {
        var query = _context.Products
            .Where(p => p.Category.Name == category)
            .AsQueryable();

        // Aplicar ordenação dinâmica
        if (!string.IsNullOrWhiteSpace(order))
        {
            query = ApplySorting(query, order);
        }

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new PaginatedResult<Product>(items, totalItems, page, size);
    }

    /// <summary>
    /// Aplica ordenação dinâmica a uma query usando uma string de ordenação no formato "campo1 asc, campo2 desc"
    /// </summary>
    private static IQueryable<Product> ApplySorting(IQueryable<Product> query, string order)
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
