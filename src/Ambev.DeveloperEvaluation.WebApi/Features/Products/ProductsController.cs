using Ambev.DeveloperEvaluation.Application.Products.CreateProduct.Commands;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct.Responses;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct.Commands;
using Ambev.DeveloperEvaluation.Application.Products.GetProductCategories;
using Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;
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
    [ProducesResponseType(StatusCodes.Status200OK)]  // Retorno bem-sucedido
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // ID inválido ou erro de validação
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Usuário não encontrado
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Erros inesperados
    public async Task<IActionResult> GetProductByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        // Validação inicial: ID não pode ser vazio
        if (id == Guid.Empty)
        {
            return BadRequest(new { Message = "Invalid product ID." });
        }

        var command = _mapper.Map<GetProductCommand>(id);
        var response = await _mediator.Send(command, cancellationToken);

        // Verifica se o usuário foi encontrado
        if (response == null)
        {
            return NotFound(new { Message = "Product not found." });
        }

        return Ok(response);
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
}
