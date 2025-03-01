namespace Ambev.DeveloperEvaluation.Application.Users.ListUsers.Responses;

public class ListUserAddressInfoResponse
{
    public string City { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public int Number { get; set; }

    public string Zipcode { get; set; } = string.Empty;

    public ListUserAddressGeolocationInfoResponse Geolocation { get; set; } = null!;
}
