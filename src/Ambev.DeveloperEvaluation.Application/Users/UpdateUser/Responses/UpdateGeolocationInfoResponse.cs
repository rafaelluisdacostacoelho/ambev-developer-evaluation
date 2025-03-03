namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Responses;

public class UpdateGeolocationInfoResponse
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
}
