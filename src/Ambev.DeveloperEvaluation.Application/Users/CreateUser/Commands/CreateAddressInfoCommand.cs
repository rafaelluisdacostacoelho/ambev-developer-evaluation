namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser.Commands;

public class CreateAddressInfoCommand
{
    public string City { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public int Number { get; set; }

    public string Zipcode { get; set; } = string.Empty;

    public CreateGeolocationInfoCommand Geolocation { get; set; } = null!;
}
