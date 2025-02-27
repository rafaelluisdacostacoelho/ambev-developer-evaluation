using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Value Object representing the user's address.
/// </summary>
[Owned]
public class AddressInfo
{
    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string Street { get; set; } = string.Empty;

    [Required]
    public int Number { get; set; }

    [Required]
    public string Zipcode { get; set; } = string.Empty;

    [Required]
    public GeolocationInfo Geolocation { get; set; } = new GeolocationInfo();
}
