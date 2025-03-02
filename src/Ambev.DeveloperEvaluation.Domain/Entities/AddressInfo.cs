namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Value Object representing the user's address.
/// </summary>
public class AddressInfo
{
    public string City { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public int Number { get; set; }

    public string Zipcode { get; set; } = string.Empty;

    public GeolocationInfo Geolocation { get; set; } = null!;

    public AddressInfo() { }

    public AddressInfo(string city, string street, int number, string zipcode, GeolocationInfo geolocation)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty.", nameof(city));

        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty.", nameof(street));

        if (number <= 0)
            throw new ArgumentOutOfRangeException(nameof(number), "Number must be greater than zero.");

        if (string.IsNullOrWhiteSpace(zipcode))
            throw new ArgumentException("Zipcode cannot be empty.", nameof(zipcode));

        City = city;
        Street = street;
        Number = number;
        Zipcode = zipcode;
        Geolocation = geolocation ?? throw new ArgumentNullException(nameof(geolocation));
    }
}
