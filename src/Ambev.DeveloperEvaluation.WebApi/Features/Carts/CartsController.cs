using Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Commands;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart.Commands;
using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

[Route("api/carts")]
public class CartsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CartsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new cart
    /// </summary>
    /// <param name="request">The cart creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created cart details</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCartAsync([FromBody] CreateCartCommand request, CancellationToken cancellationToken)
    {
        // TODO: Add UserID from Auth Context and map to UserId into Command.

        var command = _mapper.Map<CreateCartCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(nameof(GetCartByIdAsync), response);
    }

    /// <summary>
    /// Retrieves a cart by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the cart.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The cart details if found, otherwise an appropriate error response.</returns>
    [HttpGet("{id}", Name = "GetCartByIdAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCartByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(new { Message = "Invalid cart ID." });
        }

        var command = _mapper.Map<GetCartCommand>(id);
        var response = await _mediator.Send(command, cancellationToken);

        if (response == null)
        {
            return NotFound(new { Message = "Cart not found." });
        }

        return Ok(response);
    }
}
