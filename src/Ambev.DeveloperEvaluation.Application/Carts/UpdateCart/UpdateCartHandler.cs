using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart.Commands;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart.Responses;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services.Interfces;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResponse>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductPriceService _productPriceService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for UpdateUserCommand</param>
    public UpdateCartHandler(ICartRepository cartRepository, IProductPriceService productPriceService, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productPriceService = productPriceService;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the UpdateUserCommand request
    /// </summary>
    /// <param name="command">The UpdateUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated user details</returns>
    public async Task<UpdateCartResponse> Handle(UpdateCartCommand command, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Cart with ID {command.Id} not found.");

        // Obtém o preço do produto antes de adicionar ao carrinho
        foreach (var product in command.Products)
        {
            var unitPrice = await _productPriceService.GetPriceAsync(product.ProductId);

            // Adiciona o produto ao carrinho usando o método do domínio
            cart.UpdateProductQuantity(product.ProductId, product.Quantity, unitPrice);
        }

        cart.UpdateTotal();

        var updatedCart = await _cartRepository.UpdateAsync(cart, cancellationToken);

        return _mapper.Map<UpdateCartResponse>(updatedCart);
    }
}
