using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents geographical coordinates for spatial queries.
/// </summary>
[Owned]
public class GeolocationInfo
{
    /// <summary>
    /// Latitude coordinate.
    /// <para>Valid range: <c>-90</c> to <c>90</c>.</para>
    /// <para>Precision: Stores up to six decimal places.</para>
    /// </summary>
    [Required]
    [Range(-90, 90)]
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude coordinate.
    /// <para>Valid range: <c>-180</c> to <c>180</c>.</para>
    /// <para>Precision: Stores up to six decimal places.</para>
    /// </summary>
    [Required]
    [Range(-180, 180)]
    public double Longitude { get; set; }

    /// <summary>
    /// Generates a PostGIS-compatible geographical point for spatial queries.
    /// </summary>
    /// <remarks>
    /// <para><b>PostgreSQL Optimization Notes:</b></para>
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       <b>Store Latitude and Longitude as <c>double</c></b>:
    ///       PostgreSQL natively supports geospatial calculations (PostGIS),
    ///       so coordinates are stored as <c>double</c> (<c>numeric(9,6)</c>).
    ///       Six decimal places ensure a precision of approximately <b>0.11 meters</b>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <b>Enable Geospatial Indexing (Optional)</b>:
    ///       If searching for nearby users is required, switch to <b>PostGIS</b>
    ///       and use the <c>geography(Point, 4326)</c> type.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>✔ <b>Stored as <c>double</c></b>: Reduces space usage and improves performance.</para>
    /// <para>✔ <b>Range validation applied</b>: Ensures valid coordinate values.</para>
    /// </remarks>
    public Point Location => new(Longitude, Latitude) { SRID = 4326 };
}
