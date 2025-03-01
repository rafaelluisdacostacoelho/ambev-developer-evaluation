using Ambev.DeveloperEvaluation.Application.Users.CreateUser.Commands;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser.Responses;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

/// <summary>
/// Handler for processing CreateUserCommand requests
/// </summary>
public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Initializes a new instance of CreateUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for CreateUserCommand</param>
    public CreateUserHandler(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Handles the CreateUserCommand request
    /// </summary>
    /// <param name="command">The CreateUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user details</returns>
    public async Task<CreateUserResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        // Validação do comando
        //var validator = new CreateUserCommandValidator();
        //var validationResult = await validator.ValidateAsync(command, cancellationToken);

        //if (!validationResult.IsValid)
        //    throw new ValidationException(validationResult.Errors);

        // Verifica se o usuário já existe
        var existingUser = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingUser != null)
            throw new InvalidOperationException($"User with email {command.Email} already exists");

        // Mapeia o comando para a entidade User
        var user = _mapper.Map<User>(command);
        user.Password = _passwordHasher.HashPassword(command.Password); // Hash da senha

        // Persiste no repositório
        var createdUser = await _userRepository.CreateAsync(user, cancellationToken);

        var response = _mapper.Map<CreateUserResponse>(createdUser);

        return response;
    }
}
