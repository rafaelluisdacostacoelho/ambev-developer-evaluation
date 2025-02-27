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
                .HasColumnName("Firstname")
                .HasColumnType("varchar(100)")
                .IsRequired();

            name.Property(n => n.Lastname)
                .HasColumnName("Lastname")
                .HasColumnType("varchar(100)")
                .IsRequired();
        });

        // Configure Address as an Owned Type
        builder.OwnsOne(u => u.Address, address =>
        {
            address.Property(a => a.City)
                   .HasColumnName("Address_City")
                   .HasColumnType("varchar(100)")
                   .IsRequired();

            address.Property(a => a.Street)
                   .HasColumnName("Address_Street")
                   .HasColumnType("varchar(100)")
                   .IsRequired();

            address.Property(a => a.Number)
                   .HasColumnName("Address_Number")
                   .HasColumnType("int")
                   .IsRequired();

            address.Property(a => a.Zipcode)
                   .HasColumnName("Address_Zipcode")
                   .HasColumnType("varchar(20)")
                   .IsRequired();

            // Configure Geolocation as an Owned Type
            address.OwnsOne(a => a.Geolocation, geo =>
            {
                geo.Property(g => g.Latitude)
                   .HasColumnName("Geolocation_Latitude")
                   .HasColumnType("double precision")
                   .IsRequired();

                geo.Property(g => g.Longitude)
                   .HasColumnName("Geolocation_Longitude")
                   .HasColumnType("double precision")
                   .IsRequired();

                geo.Property(g => g.Location)
                   .HasColumnName("Geolocation_Location")
                   .HasColumnType("geography(Point, 4326)")
                   .IsRequired();
            });
        });
    }
}
