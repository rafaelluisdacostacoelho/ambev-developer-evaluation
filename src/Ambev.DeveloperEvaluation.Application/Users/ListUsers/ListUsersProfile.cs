using Ambev.DeveloperEvaluation.Application.Users.ListUsers.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

/// <summary>
/// Profile for mapping between User entity and GetUserResponse
/// </summary>
public class ListUsersProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUser operation
    /// </summary>
    public ListUsersProfile()
    {
        // Mapeamento de User para ListUsersResponse
        CreateMap<User, ListUserResponse>();

        // Mapeamento direto entre User e ListUserResponse
        CreateMap<User, ListUserResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) // Nome
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address)) // Endereço
            .ForMember(dest => dest.Password, opt => opt.Ignore()); // Evitar expor a senha

        // Mapeamento entre NameInfo e ListUserNameInfoResponse
        CreateMap<NameInfo, ListUserNameInfoResponse>();

        // Mapeamento entre AddressInfo e ListUserAddressInfoResponse
        CreateMap<AddressInfo, ListUserAddressInfoResponse>()
            .ForMember(dest => dest.Geolocation, opt => opt.MapFrom(src => src.Geolocation)); // Mapeia a geolocalização corretamente

        // Mapeamento entre GeolocationInfo e ListUserAddressGeolocationInfo
        CreateMap<GeolocationInfo, ListUserAddressGeolocationInfoResponse>();

        // Mapeamento de ICollection<User> para ICollection<ListUsersResponse>
        CreateMap<ICollection<User>, ICollection<ListUserResponse>>()
            .ConvertUsing((src, dest, context) => [.. src.Select(user => context.Mapper.Map<ListUserResponse>(user))]);
    }
}
