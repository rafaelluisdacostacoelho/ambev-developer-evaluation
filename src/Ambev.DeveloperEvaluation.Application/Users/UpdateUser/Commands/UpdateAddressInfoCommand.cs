namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Commands;

/// <summary>
/// Command to provide category information for a user
/// </summary>
public class UpdateAddressInfoCommand
{
    public string City { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public int Number { get; set; }

    public string Zipcode { get; set; } = string.Empty;

    public UpdateGeolocationInfoCommand Geolocation { get; set; } = null!;

    public UpdateAddressInfoCommand(string city, string street, int number, string zipcode, UpdateGeolocationInfoCommand geolocation)
    {
        City = city;
        Street = street;
        Number = number;
        Zipcode = zipcode;
        Geolocation = geolocation;
    }
}
