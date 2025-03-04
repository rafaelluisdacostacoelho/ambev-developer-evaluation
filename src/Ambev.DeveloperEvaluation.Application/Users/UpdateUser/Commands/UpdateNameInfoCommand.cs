namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser.Commands;

/// <summary>
/// Command to provide rating information for a product
/// </summary>
public class UpdateNameInfoCommand
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;

    public UpdateNameInfoCommand(string firstname, string lastname)
    {
        Firstname = firstname;
        Lastname = lastname;
    }
}
