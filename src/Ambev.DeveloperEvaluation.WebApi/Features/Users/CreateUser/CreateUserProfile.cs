using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

/// <summary>
/// Profile for mapping between Application and API CreateUser responses
/// </summary>
public class CreateUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser feature
    /// </summary>
    public CreateUserProfile()
    {
        CreateMap<CreateUserRequest, CreateUserCommand>();
        CreateMap<CreateNameInfoRequest, CreateNameInfoCommand>();
        CreateMap<CreateAddressInfoRequest, CreateAddressInfoCommand>();
        CreateMap<CreateGeolocationInfoRequest, CreateGeolocationInfoCommand>();

        CreateMap<CreateUserResult, CreateUserResponse>();
    }
}
