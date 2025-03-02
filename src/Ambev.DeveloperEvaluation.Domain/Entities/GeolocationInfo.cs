namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents geographical coordinates for spatial queries.
/// </summary>
public class GeolocationInfo
{
    /// <summary>
    /// Latitude coordinate.
    /// <para>Valid range: <c>-90</c> to <c>90</c>.</para>
    /// <para>Precision: Stores up to six decimal places.</para>
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude coordinate.
    /// <para>Valid range: <c>-180</c> to <c>180</c>.</para>
    /// <para>Precision: Stores up to six decimal places.</para>
    /// </summary>
    public double Longitude { get; set; }


    // Construtor privado para ORMs
    public GeolocationInfo() { }

    public GeolocationInfo(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90.");

        if (longitude < -180 || longitude > 180)
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180.");

        Latitude = latitude;
        Longitude = longitude;
    }
}
