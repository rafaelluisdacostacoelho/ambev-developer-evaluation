using Ambev.DeveloperEvaluation.Application.Users.CreateUser.Commands;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser.Responses;

public class GetAddressInfoResponse
{
    public string City { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public int Number { get; set; }

    public string Zipcode { get; set; } = string.Empty;

    public GetGeolocationInfoResponse Geolocation { get; set; } = null!;
}
