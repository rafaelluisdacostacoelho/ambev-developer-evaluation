using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Pagination;
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

    public async Task<PaginatedResult<Product>> GetPaginatedAsync(int page = 1, int size = 10, string? order = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Products.AsQueryable();

        // Aplicar ordenação dinâmica
        if (!string.IsNullOrWhiteSpace(order))
        {
            query = ApplySorting(query, order);
        }

        // Contagem total de itens antes da paginação
        var totalItems = await query.CountAsync(cancellationToken: cancellationToken);

        // Aplicar paginação
        var items = await query.Skip((page - 1) * size)
                               .Take(size)
                               .ToListAsync(cancellationToken: cancellationToken);

        return new PaginatedResult<Product>
        {
            Items = items,
            TotalItems = totalItems,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalItems / (double)size)
        };
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        var existingProduct = await _context.Products.FindAsync([product.Id], cancellationToken: cancellationToken)
            ?? throw new KeyNotFoundException("Produto não encontrado.");
        _context.Entry(existingProduct).CurrentValues.SetValues(product);

        // Atualiza propriedades complexas manualmente
        existingProduct.UpdateCategory(new CategoryInfo(product.Category.ExternalId, product.Category.Name));
        existingProduct.UpdateRating(new RatingInfo(product.Rating.ExternalId, product.Rating.AverageRate, product.Rating.TotalReviews));

        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Products.FindAsync([id], cancellationToken);
        if (entity == null)
            return false;
        var product = await _context.Products.FindAsync([id], cancellationToken: cancellationToken)
            ?? throw new KeyNotFoundException("Produto não encontrado.");
        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<List<string>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
                             .Select(p => p.Category.Name)
                             .Distinct()
                             .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<PaginatedResult<Product>> GetProductsByCategoryAsync(string category, int page = 1, int size = 10, string? order = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Products
                            .Where(p => p.Category.Name == category)
                            .AsQueryable();

        // Aplicar ordenação dinâmica
        if (!string.IsNullOrWhiteSpace(order))
        {
            query = ApplySorting(query, order);
        }

        var totalItems = await query.CountAsync(cancellationToken: cancellationToken);

        var items = await query.Skip((page - 1) * size)
                               .Take(size)
                               .ToListAsync(cancellationToken: cancellationToken);

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
