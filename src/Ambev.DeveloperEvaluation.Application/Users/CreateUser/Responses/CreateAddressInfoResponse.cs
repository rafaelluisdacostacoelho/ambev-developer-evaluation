namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser.Responses;

public class CreateAddressInfoResponse
{
    public string City { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public int Number { get; set; }

    public string Zipcode { get; set; } = string.Empty;

    public CreateGeolocationInfoResponse Geolocation { get; set; } = null!;
}
