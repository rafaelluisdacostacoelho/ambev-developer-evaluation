using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart.Commands;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

/// <summary>
/// AutoMapper profile to configure mappings for the UpdateProduct feature
/// </summary>
public class UpdateCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateCart feature
    /// </summary>
    public UpdateCartProfile()
    {
        CreateMap<Cart, UpdateCartResponse>();

        // Mapeamento do comando para a entidade Cart utilizando os métodos de domínio
        CreateMap<UpdateCartCommand, Cart>()
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
        CreateMap<UpdateCartItemCommand, CartItem>()
            .ConstructUsing(src => new CartItem(src.ProductId, src.Quantity));
    }
}
