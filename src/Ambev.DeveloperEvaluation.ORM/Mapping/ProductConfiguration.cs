using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(u => u.Id)
               .HasColumnType("uuid")
               .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.Title)
               .HasColumnType("text")
               .IsRequired();

        builder.Property(p => p.Price)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(p => p.Description)
               .HasColumnType("text");

        builder.Property(p => p.Image)
               .HasColumnType("text");

        // Configurando Category como Owned Type
        builder.OwnsOne(p => p.Category, category =>
        {
            category.Property(c => c.ExternalId)
                    .HasColumnName("category_external_id")
                    .HasColumnType("text");

            category.Property(c => c.Name)
                    .HasColumnName("category_name")
                    .HasColumnType("text");
        });

        // Configurando Rating como Owned Type
        builder.OwnsOne(p => p.Rating, rating =>
        {
            rating.Property(r => r.ExternalId)
                  .HasColumnName("rating_external_id")
                  .HasColumnType("text");

            rating.Property(r => r.AverageRate)
                  .HasColumnName("rating_average_rate")
                  .HasColumnType("decimal(3,1)");

            rating.Property(r => r.TotalReviews)
                  .HasColumnName("rating_total_reviews")
                  .HasColumnType("int");
        });
    }
}
