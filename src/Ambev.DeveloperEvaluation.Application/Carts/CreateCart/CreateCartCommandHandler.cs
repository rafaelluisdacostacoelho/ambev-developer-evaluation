using Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Commands;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

/// <summary>
/// Handler for processing CreateCartCommand requests
/// </summary>
public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, CreateCartResponse>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CreateCartCommandHandler.
    /// </summary>
    /// <param name="cartRepository">The cart repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public CreateCartCommandHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CreateCartCommand request.
    /// </summary>
    /// <param name="command">The CreateCart command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created cart details.</returns>
    public async Task<CreateCartResponse> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        // Map the CreateCartCommand to the Cart entity using AutoMapper
        var cart = _mapper.Map<Cart>(command);

        // Persist the cart entity in the repository
        var createdCart = await _cartRepository.CreateAsync(cart, cancellationToken);

        // Map the result to the response DTO
        var response = _mapper.Map<CreateCartResponse>(createdCart);

        return response;
    }
}
