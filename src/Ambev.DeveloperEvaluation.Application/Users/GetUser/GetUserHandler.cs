using Ambev.DeveloperEvaluation.Application.Users.GetUser.Commands;
using Ambev.DeveloperEvaluation.Application.Users.GetUser.Responses;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

/// <summary>
/// Handler for processing GetUserCommand requests
/// </summary>
public class GetUserHandler : IRequestHandler<GetUserCommand, GetUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserHandler> _logger;
    /// <summary>
    /// Initializes a new instance of GetUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for GetUserCommand</param>
    public GetUserHandler(IUserRepository userRepository, IMapper mapper, ILogger<GetUserHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the GetUserCommand request
    /// </summary>
    /// <param name="request">The GetUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user details if found</returns>
    public async Task<GetUserResponse> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetUserCommand for User ID {UserId}", request.Id);

        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", request.Id);
            throw new KeyNotFoundException($"User with ID {request.Id} not found");
        }

        _logger.LogInformation("Successfully handled GetUserCommand for User ID {UserId}", request.Id);

        return _mapper.Map<GetUserResponse>(user);
    }
}
