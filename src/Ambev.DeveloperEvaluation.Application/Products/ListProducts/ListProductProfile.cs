using Ambev.DeveloperEvaluation.Application.Products.ListProducts.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Profile for mapping between Product entity and GetProductResponse
/// </summary>
public class ListProductsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetProduct operation
    /// </summary>
    public ListProductsProfile()
    {
        // Mapeamento de Product para ListProductResponse
        CreateMap<Product, ListProductResponse>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));

        // Mapeamento entre CategoryInfo e ListProductCategoryInfoResponse
        CreateMap<CategoryInfo, ListProductCategoryInfoResponse>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        // Mapeamento entre RatingInfo e ListProductRatingInfoResponse
        CreateMap<RatingInfo, ListProductRatingInfoResponse>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.AverageRate, opt => opt.MapFrom(src => src.AverageRate))
            .ForMember(dest => dest.TotalReviews, opt => opt.MapFrom(src => src.TotalReviews));

        // Mapeamento de ICollection<Product> para ICollection<ListProductResponse>
        CreateMap<ICollection<Product>, ICollection<ListProductResponse>>()
            .ConvertUsing((src, dest, context) => [.. src.Select(product => context.Mapper.Map<ListProductResponse>(product))]);
    }
}
