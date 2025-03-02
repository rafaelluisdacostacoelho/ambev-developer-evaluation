using Ambev.DeveloperEvaluation.Application.Pagination;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct.Commands;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct.Responses;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct.Commands;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct.Commands;
using Ambev.DeveloperEvaluation.Application.Products.GetProductCategories;
using Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts.Responses;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct.Commands;
using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

[Route("api/products")]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="request">The product creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created product details</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductCommand request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateProductCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(nameof(GetProductByIdAsync), _mapper.Map<CreateProductResponse>(response));
    }

    /// <summary>
    /// Retrieves a product by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The product details if found, otherwise an appropriate error response.</returns>
    [HttpGet("{id}", Name = "GetProductByIdAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(new { Message = "Invalid product ID." });
        }

        var command = _mapper.Map<GetProductCommand>(id);
        var response = await _mediator.Send(command, cancellationToken);

        if (response == null)
        {
            return NotFound(new { Message = "Product not found." });
        }

        return Ok(response);
    }

    /// <summary>
    /// Retrieves a paginated list of products.
    /// </summary>
    /// <param name="pageNumber">Page number (must be 1 or greater).</param>
    /// <param name="pageSize">Number of products per page (must be between 1 and 100).</param>
    /// <param name="order">Sorting order (optional).</param>
    /// <param name="filter">Filters to apply (optional).</param>
    /// <returns>Paginated list of products.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductsPageAsync([FromQuery] int pageNumber = 1,
                                                          [FromQuery] int pageSize = 10,
                                                          [FromQuery] string? order = null,
                                                          [FromQuery] ListProductsQuery? filter = null)
    {
        if (pageNumber < 1)
        {
            return BadRequest(new { Message = "Page number must be greater than or equal to 1." });
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest(new { Message = "Page size must be between 1 and 100." });
        }

        var query = new PaginationQuery<ListProductsQuery, ListProductResponse>(pageNumber, pageSize, order, filter);

        PaginatedResponse<ListProductResponse> result = await _mediator.Send(query);

        return OkPaginated(result);
    }


    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _mediator.Send(new GetProductCategoriesQuery());
        return Ok(categories);
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetProductsByCategory(string category,
                                                           [FromQuery] int page = 1,
                                                           [FromQuery] int size = 10,
                                                           [FromQuery] string? order = null)
    {
        var query = new GetProductsByCategoryQuery(category, page, size, order);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="id">The unique identifier of the product to update</param>
    /// <param name="request">The product update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated product details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid id, [FromBody] UpdateProductCommand request, CancellationToken cancellationToken)
    {
        // Garante que o ID da rota seja utilizado no comando
        request.Id = id;

        // Envia o comando direto ao Mediator, confiando que os middlewares e validators já garantem a integridade dos dados
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Deletes a product by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content if the product was deleted, or an error response.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProductAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<DeleteProductCommand>(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result == null)
        {
            return NotFound(new { Message = "Product not found." });
        }

        return NoContent();
    }
}
