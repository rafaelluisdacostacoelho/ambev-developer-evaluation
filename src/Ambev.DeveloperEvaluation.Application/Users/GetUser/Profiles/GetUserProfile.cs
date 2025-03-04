using Ambev.DeveloperEvaluation.Application.Users.CreateUser.Commands;
using Ambev.DeveloperEvaluation.Application.Users.GetUser.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser.Profiles;

/// <summary>
/// Profile for mapping between User entity and GetUserResponse
/// </summary>
public class GetUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUser operation
    /// </summary>
    public GetUserProfile()
    {
        CreateMap<User, GetUserResponse>();

        CreateMap<AddressInfo, GetAddressInfoResponse>();
        CreateMap<NameInfo, GetNameInfoResponse>();
        CreateMap<GeolocationInfo, GetGeolocationInfoResponse>();
    }
}
