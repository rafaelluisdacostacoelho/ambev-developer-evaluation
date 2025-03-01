namespace Ambev.DeveloperEvaluation.Application.Pagination;

public class PaginatedResponse<T>
{
    public ICollection<T> Data { get; set; } = [];
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
}
