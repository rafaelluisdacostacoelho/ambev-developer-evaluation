namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Value Object representing the user's full name.
/// </summary>
public class NameInfo
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;

    public NameInfo() { }

    public NameInfo(string firstname, string lastname)
    {
        if (string.IsNullOrWhiteSpace(firstname))
            throw new ArgumentException("Firstname cannot be empty.", nameof(firstname));

        if (string.IsNullOrWhiteSpace(lastname))
            throw new ArgumentException("Lastname cannot be empty.", nameof(lastname));

        Firstname = firstname;
        Lastname = lastname;
    }
}
