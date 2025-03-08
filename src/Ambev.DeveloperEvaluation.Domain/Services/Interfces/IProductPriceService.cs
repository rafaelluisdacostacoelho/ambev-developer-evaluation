namespace Ambev.DeveloperEvaluation.Domain.Services.Interfces;

public interface IProductPriceService
{
    Task<decimal> GetPriceAsync(Guid productId);
}
