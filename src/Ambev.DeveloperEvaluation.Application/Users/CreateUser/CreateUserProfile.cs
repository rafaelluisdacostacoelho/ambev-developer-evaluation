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
            .ForMember(dest => dest.Password, opt => opt.Ignore()) // Senha será tratada no handler
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()); // Será atualizado apenas quando necessário;

        CreateMap<CreateAddressInfoCommand, AddressInfo>();
        CreateMap<CreateNameInfoCommand, NameInfo>();
        CreateMap<CreateGeolocationInfoCommand, GeolocationInfo>();

        CreateMap<User, CreateUserResult>();
    }
}
