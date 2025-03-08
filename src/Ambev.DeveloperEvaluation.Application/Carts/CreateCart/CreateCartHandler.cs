using Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Commands;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services.Interfces;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

/// <summary>
/// Handler for processing CreateCartCommand requests
/// </summary>
public class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResponse>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductPriceService _productPriceService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CreateCartCommandHandler.
    /// </summary>
    /// <param name="cartRepository">The cart repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public CreateCartHandler(ICartRepository cartRepository, IProductPriceService productPriceService, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _productPriceService = productPriceService;
    }

    /// <summary>
    /// Handles the CreateCartCommand request.
    /// </summary>
    /// <param name="command">The CreateCart command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created cart details.</returns>
    public async Task<CreateCartResponse> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        var cart = new Cart(command.UserId);

        // Obtém o preço do produto antes de adicionar ao carrinho
        foreach (var product in command.Products)
        {
            var unitPrice = await _productPriceService.GetPriceAsync(product.ProductId);

            // Adiciona o produto ao carrinho usando o método do domínio
            cart.UpdateProduct(product.ProductId, product.Quantity, unitPrice);
        }

        // Persist the cart entity in the repository
        var createdCart = await _cartRepository.CreateAsync(cart, cancellationToken);

        // Map the result to the response DTO
        var response = _mapper.Map<CreateCartResponse>(createdCart);

        return response;
    }
}
