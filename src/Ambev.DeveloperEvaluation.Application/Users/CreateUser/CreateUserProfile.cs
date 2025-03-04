using Ambev.DeveloperEvaluation.Application.Users.CreateUser.Commands;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

/// <summary>
/// Profile for mapping between User entity and CreateUserResponse
/// </summary>
public class CreateUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser operation
    /// </summary>
    public CreateUserProfile()
    {
        CreateMap<CreateUserCommand, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<CreateNameInfoCommand, NameInfo>();

        CreateMap<CreateGeolocationInfoCommand, GeolocationInfo>()
            .ConstructUsing(src => new GeolocationInfo(src.Latitude, src.Longitude));

        CreateMap<CreateAddressInfoCommand, AddressInfo>()
            .ConstructUsing(src => new AddressInfo(
                src.City,
                src.Street,
                src.Number,
                src.Zipcode,
                new GeolocationInfo(src.Geolocation.Latitude, src.Geolocation.Longitude)
            ));

        CreateMap<User, CreateUserResponse>();

        CreateMap<AddressInfo, CreateAddressInfoResponse>();
        CreateMap<NameInfo, CreateNameInfoResponse>();
        CreateMap<GeolocationInfo, CreateGeolocationInfoResponse>();
    }
}
