using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Ambev.DeveloperEvaluation.ORM.Configurations;

internal class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.SaleNumber)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(u => u.Id)
               .HasColumnType("uuid")
               .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(u => u.SaleDate)
               .IsRequired()
               .HasColumnType("timestamp")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(u => u.UserId)
               .IsRequired();

        builder.Property(u => u.PriceTotal)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(u => u.CartId)
               .IsRequired();
    }
}