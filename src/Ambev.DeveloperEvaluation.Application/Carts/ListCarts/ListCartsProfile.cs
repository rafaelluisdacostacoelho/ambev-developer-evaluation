using Ambev.DeveloperEvaluation.Application.Carts.ListCarts.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

/// <summary>
/// Profile for mapping between Cart entity and GetCartResponse
/// </summary>
public class ListCartsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetCart operation
    /// </summary>
    public ListCartsProfile()
    {
        CreateMap<Cart, ListCartResponse>()
             .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        CreateMap<CartItem, ListCartItemResponse>();
    }
}
