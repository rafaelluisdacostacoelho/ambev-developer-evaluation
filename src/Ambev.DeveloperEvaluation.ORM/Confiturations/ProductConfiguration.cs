using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Confiturations;

/// <summary>
/// Configuration for the Product entity in PostgreSQL.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        // Primary Key
        builder.HasKey(p => p.Id);

        // Configure ID as UUID with default value
        builder.Property(p => p.Id)
               .HasColumnType("uuid")
               .HasDefaultValueSql("gen_random_uuid()");

        // Title (Indexed for better search performance)
        builder.Property(p => p.Title)
               .HasColumnType("varchar(100)")
               .IsRequired();
        builder.HasIndex(p => p.Title);

        // Price (Optimized decimal type for PostgreSQL)
        builder.Property(p => p.Price)
               .HasColumnType("numeric(10,2)")
               .IsRequired();

        // Description (Text optimized for PostgreSQL)
        builder.Property(p => p.Description)
               .HasColumnType("text");

        // Image (URL with reasonable size limit)
        builder.Property(p => p.Image)
               .HasColumnType("text");

        // Configure Category as an Owned Type
        builder.OwnsOne(p => p.Category, category =>
        {
            category.Property(c => c.ExternalId)
                    .HasColumnName("Category_ExternalId")
                    .HasColumnType("varchar(50)"); // Restricting ID size

            category.Property(c => c.Name)
                    .HasColumnName("Category_Name")
                    .HasColumnType("varchar(100)"); // Limiting Name size
        });

        // Configure Rating as an Owned Type
        builder.OwnsOne(p => p.Rating, rating =>
        {
            rating.Property(r => r.ExternalId)
                  .HasColumnName("Rating_ExternalId")
                  .HasColumnType("varchar(50)");

            rating.Property(r => r.AverageRate)
                  .HasColumnName("Rating_AverageRate")
                  .HasColumnType("numeric(3,1)"); // Precision limit

            rating.Property(r => r.TotalReviews)
                  .HasColumnName("Rating_TotalReviews")
                  .HasColumnType("int");
        });
    }
}
