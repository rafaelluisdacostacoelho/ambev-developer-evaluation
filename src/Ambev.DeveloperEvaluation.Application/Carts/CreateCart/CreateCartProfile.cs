using Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Commands;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

/// <summary>
/// Profile for mapping between Cart entity and CreateCartResponse
/// </summary>
public class CreateCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateCart operation
    /// </summary>
    public CreateCartProfile()
    {
        CreateMap<Cart, CreateCartResponse>();

        // Mapeamento do comando para a entidade Cart utilizando os métodos de domínio
        CreateMap<CreateCartCommand, Cart>()
            .ConstructUsing((command, context) =>
            {
                var cart = new Cart(command.UserId);

                // Adiciona produtos ao carrinho usando o método de domínio
                foreach (var product in command.Products)
                {
                    cart.AddProduct(product.ProductId, product.Quantity);
                }

                return cart;
            });

        // Mapeamento do comando do item do carrinho para a entidade CartItem
        CreateMap<CreateCartItemCommand, CartItem>()
            .ConstructUsing(src => new CartItem(src.ProductId, src.Quantity));
    }
}
