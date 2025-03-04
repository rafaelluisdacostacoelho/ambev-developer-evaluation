using Ambev.DeveloperEvaluation.Domain.Enums;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser.Responses;

/// <summary>
/// Response model for GetUser operation
/// </summary>
public class GetUserResponse
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the username of the user to be created.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password for the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number for the user.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address for the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the user.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the role of the user.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role { get; set; }

    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    public GetNameInfoResponse Name { get; set; } = new GetNameInfoResponse();

    /// <summary>
    /// Gets or sets the address of the user.
    /// </summary>
    public GetAddressInfoResponse Address { get; set; } = new GetAddressInfoResponse();
}
