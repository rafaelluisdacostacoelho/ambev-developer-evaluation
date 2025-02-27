using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for the User entity in PostgreSQL.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        // Primary Key
        builder.HasKey(u => u.Id);

        // Configure ID as UUID with default value
        builder.Property(u => u.Id)
               .HasColumnType("uuid")
               .HasDefaultValueSql("gen_random_uuid()");

        // Username
        builder.Property(u => u.Username)
               .HasColumnType("varchar(50)")
               .IsRequired();
        builder.HasIndex(u => u.Username).IsUnique();

        // Email
        builder.Property(u => u.Email)
               .HasColumnType("varchar(255)")
               .IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();

        // Password
        builder.Property(u => u.Password)
               .HasColumnType("varchar(100)")
               .IsRequired();

        // Phone
        builder.Property(u => u.Phone)
               .HasColumnType("varchar(20)");
        builder.HasIndex(u => u.Phone); // Não único, mas otimiza busca

        // Status
        builder.Property(u => u.Status)
               .HasColumnType("varchar(15)")
               .HasConversion<string>()
               .IsRequired();

        // Role
        builder.Property(u => u.Role)
               .HasColumnType("varchar(15)")
               .HasConversion<string>()
               .IsRequired();

        // CreatedAt e UpdatedAt
        builder.Property(u => u.CreatedAt)
               .HasColumnType("timestamp with time zone")
               .IsRequired();

        builder.Property(u => u.UpdatedAt)
               .HasColumnType("timestamp with time zone");

        // Configure Name as an Owned Type
        builder.OwnsOne(u => u.Name, name =>
        {
            name.Property(n => n.Firstname)
                .HasColumnName("firstname")
                .HasColumnType("varchar(100)")
                .IsRequired();

            name.Property(n => n.Lastname)
                .HasColumnName("lastname")
                .HasColumnType("varchar(100)")
                .IsRequired();
        });

        // Configure Address as an Owned Type
        builder.OwnsOne(u => u.Address, address =>
        {
            address.Property(a => a.City)
                   .HasColumnName("city")
                   .HasColumnType("varchar(100)")
                   .IsRequired();

            address.Property(a => a.Street)
                   .HasColumnName("street")
                   .HasColumnType("varchar(100)")
                   .IsRequired();

            address.Property(a => a.Number)
                   .HasColumnName("number")
                   .HasColumnType("int")
                   .IsRequired();

            address.Property(a => a.Zipcode)
                   .HasColumnName("zipcode")
                   .HasColumnType("varchar(20)")
                   .IsRequired();

            // Configure Geolocation as an Owned Type
            address.OwnsOne(a => a.Geolocation, geo =>
            {
                geo.Property(g => g.Latitude)
                   .HasColumnName("geo_latitude")
                   .HasColumnType("double precision")
                   .IsRequired();

                geo.Property(g => g.Longitude)
                   .HasColumnName("geo_longitude")
                   .HasColumnType("double precision")
                   .IsRequired();

                geo.Property(g => g.Location)
                   .HasColumnName("geo_location")
                   .HasColumnType("geography(Point, 4326)")
                   .IsRequired();
            });
        });
    }
}
