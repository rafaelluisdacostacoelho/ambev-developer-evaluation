using Ambev.DeveloperEvaluation.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Cart : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    public List<CartItem> Products { get; set; } = [];
}
