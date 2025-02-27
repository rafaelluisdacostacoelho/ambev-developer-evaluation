using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Value Object representing the user's full name.
/// </summary>
[Owned]
public class NameInfo
{
    [Required]
    public string Firstname { get; set; } = string.Empty;

    [Required]
    public string Lastname { get; set; } = string.Empty;
}
