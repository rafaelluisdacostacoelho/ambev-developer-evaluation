using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
               .HasColumnType("uuid")
               .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.UserId)
               .HasColumnType("uuid")
               .IsRequired();

        builder.Property(c => c.Date)
               .HasColumnType("timestamp")
               .IsRequired();

        // Configurando CartItem como Owned Type
        builder.OwnsMany(c => c.Products, cartItem =>
        {
            cartItem.Property(ci => ci.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("uuid");

            cartItem.Property(ci => ci.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int");
        });
    }
}
