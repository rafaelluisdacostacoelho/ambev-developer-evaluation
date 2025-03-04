namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Responses;

public class UpdateAddressInfoResponse
{
    public string City { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public int Number { get; set; }

    public string Zipcode { get; set; } = string.Empty;

    public UpdateGeolocationInfoResponse Geolocation { get; set; } = null!;
}
