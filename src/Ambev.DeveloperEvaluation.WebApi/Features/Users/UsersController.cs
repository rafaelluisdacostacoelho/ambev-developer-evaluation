using Ambev.DeveloperEvaluation.Application.Pagination;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser.Commands;
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser.Commands;
using Ambev.DeveloperEvaluation.Application.Users.GetUser.Commands;
using Ambev.DeveloperEvaluation.Application.Users.ListUsers;
using Ambev.DeveloperEvaluation.Application.Users.ListUsers.Responses;
using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users;

/// <summary>
/// Controller for managing user operations
/// </summary>
[Route("api/users")]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UsersController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UsersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created user details.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        if (response == null)
        {
            return Conflict(new { Message = "User already exists." });
        }

        return Created(nameof(GetUserByIdAsync), new { id = response.Id }, response);
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user details if found, otherwise an appropriate error response.</returns>
    [HttpGet("{id}", Name = "GetUserByIdAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(new { Message = "Invalid user ID." });
        }

        var command = _mapper.Map<GetUserCommand>(id);
        var response = await _mediator.Send(command, cancellationToken);

        if (response == null)
        {
            return NotFound(new { Message = "User not found." });
        }

        return Ok(response);
    }

    /// <summary>
    /// Retrieves a paginated list of users.
    /// </summary>
    /// <param name="pageNumber">Page number (must be 1 or greater).</param>
    /// <param name="pageSize">Number of users per page (must be between 1 and 100).</param>
    /// <param name="order">Sorting order (optional).</param>
    /// <param name="filter">Filters to apply (optional).</param>
    /// <returns>Paginated list of users.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUsersPageAsync([FromQuery] int pageNumber = 1,
                                                       [FromQuery] int pageSize = 10,
                                                       [FromQuery] string? order = null,
                                                       [FromQuery] ListUsersFilter? filter = null)
    {
        if (pageNumber < 1)
        {
            return BadRequest(new { Message = "Page number must be greater than or equal to 1." });
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest(new { Message = "Page size must be between 1 and 100." });
        }

        var query = new PaginationQuery<ListUsersFilter, ListUserResponse>(pageNumber, pageSize, order, filter);

        PaginatedResponse<ListUserResponse> result = await _mediator.Send(query);

        return OkPaginated(result);
    }


    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content if the user was deleted, or an error response.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<DeleteUserCommand>(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result == null)
        {
            return NotFound(new { Message = "User not found." });
        }

        return NoContent();
    }
}
