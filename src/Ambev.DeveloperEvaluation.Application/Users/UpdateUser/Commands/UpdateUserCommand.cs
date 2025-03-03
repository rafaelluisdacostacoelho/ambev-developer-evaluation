using Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Responses;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Commands;

/// <summary>
/// Command to create a new product
/// </summary>
public class UpdateUserCommand : IRequest<UpdateUserResponse>
{
    public Guid Id { get; set; }

    /// <summary>
    /// The username of the user.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The phone number of the user.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// The hashed password for authentication.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// The user's role in the system.
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// The user's account status.
    /// </summary>
    public UserStatus Status { get; set; }

    /// <summary>
    /// The user's full name.
    /// </summary>
    public UpdateNameInfoCommand Name { get; set; } = null!;

    /// <summary>
    /// The user's address.
    /// </summary>
    public UpdateAddressInfoCommand Address { get; set; } = null!;

    public UpdateUserCommand(string username, string email, string phone, string password, UserRole role, UserStatus status, UpdateNameInfoCommand name, UpdateAddressInfoCommand address)
    {
        Username = username;
        Email = email;
        Phone = phone;
        Password = password;
        Role = role;
        Status = status;
        Address = address;
        Name = name;
    }
}
