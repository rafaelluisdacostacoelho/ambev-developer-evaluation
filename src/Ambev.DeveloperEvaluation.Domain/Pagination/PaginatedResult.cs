namespace Ambev.DeveloperEvaluation.Domain.Pagination;

public class PaginatedResult<T>
{
    public ICollection<T> Items { get; set; } = [];
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PaginatedResult()
    {
        Items = [];
    }

    public PaginatedResult(ICollection<T> items, int totalItems, int currentPage, int size)
    {
        Items = items;
        TotalItems = totalItems;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(totalItems / (double)size);
    }
}
