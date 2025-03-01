namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser.Requests;

public class CreateAddressInfoRequest
{
    public string City { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public int Number { get; set; }

    public string Zipcode { get; set; } = string.Empty;

    public CreateGeolocationInfoRequest Geolocation { get; set; } = null!;
}
