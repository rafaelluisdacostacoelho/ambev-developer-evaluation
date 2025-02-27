namespace Ambev.DeveloperEvaluation.Common.Pagination;

public class PaginatedResult<T>
{
    public List<T> Data { get; set; }
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public PaginatedResult()
    {
        Data = [];
    }

    public PaginatedResult(List<T> data, int totalItems, int currentPage, int size)
    {
        Data = data;
        TotalItems = totalItems;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(totalItems / (double)size);
    }
}
