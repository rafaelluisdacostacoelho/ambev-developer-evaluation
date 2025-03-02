using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct.Commands;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// AutoMapper profile to configure mappings for the CreateProduct feature
/// </summary>
public class UpdateProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateProduct feature
    /// </summary>
    public UpdateProductProfile()
    {
        CreateMap<UpdateProductCommand, Product>();
        CreateMap<UpdateRatingInfoCommand, RatingInfo>();
        CreateMap<UpdateCategoryInfoCommand, CategoryInfo>();

        CreateMap<Product, UpdateProductResponse>();
    }
}
