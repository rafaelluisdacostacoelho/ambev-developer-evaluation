namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct.Commands;

/// <summary>
/// Command to provide category information for a product
/// </summary>
public class CreateCategoryInfoCommand
{
    /// <summary>
    /// The external identifier for the category
    /// </summary>
    public string ExternalId { get; private set; } = string.Empty;

    /// <summary>
    /// The name of the category
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    public CreateCategoryInfoCommand(string externalId, string name)
    {
        ExternalId = externalId;
        Name = name;
    }
}
