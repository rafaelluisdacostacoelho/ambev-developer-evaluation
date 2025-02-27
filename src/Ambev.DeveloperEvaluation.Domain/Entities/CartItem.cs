using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

[Owned]
public class CartItem
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [Range(1, 20)]
    public int Quantity { get; set; }
}
