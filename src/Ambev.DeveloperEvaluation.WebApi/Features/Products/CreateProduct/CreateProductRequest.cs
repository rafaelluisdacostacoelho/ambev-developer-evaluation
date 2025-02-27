namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductRequest
{
    /// <summary>
    /// 
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Image { get; set; } = string.Empty;
}
