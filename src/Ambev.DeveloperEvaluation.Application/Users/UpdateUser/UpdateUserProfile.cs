using Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Commands;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// AutoMapper profile to configure mappings for the CreateUser feature
/// </summary>
public class UpdateUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser feature
    /// </summary>
    public UpdateUserProfile()
    {
        CreateMap<UpdateUserCommand, User>();
        CreateMap<UpdateNameInfoCommand, NameInfo>();
        CreateMap<UpdateAddressInfoCommand, AddressInfo>();
        CreateMap<UpdateGeolocationInfoCommand, GeolocationInfo>();

        CreateMap<User, UpdateUserResponse>();
        CreateMap<AddressInfo, UpdateAddressInfoResponse>();
        CreateMap<NameInfo, UpdateNameInfoResponse>();
        CreateMap<GeolocationInfo, UpdateGeolocationInfoResponse>();
    }
}
