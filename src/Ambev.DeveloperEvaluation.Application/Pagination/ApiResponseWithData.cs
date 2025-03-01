namespace Ambev.DeveloperEvaluation.Application.Pagination;

public class ApiResponseWithData<T> : ApiResponse
{
    public T? Data { get; set; }
}
