using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasColumnType("uuid")
               .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(u => u.Username)
               .HasColumnType("text")
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(u => u.PasswordHash)
               .HasColumnType("text")
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.Email)
               .HasColumnType("text")
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.Phone)
               .HasColumnType("text")
               .HasMaxLength(20);

        builder.Property(u => u.Status)
               .HasColumnType("text")
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.Property(u => u.Role)
               .HasColumnType("text")
               .HasConversion<string>()
               .HasMaxLength(20);

        // Configurando NameInfo como Owned Type
        builder.OwnsOne(u => u.Name, name =>
        {
            name.Property(n => n.Firstname)
                .HasColumnName("firstname")
                .HasColumnType("text");

            name.Property(n => n.Lastname)
                .HasColumnName("lastname")
                .HasColumnType("text");
        });

        // Configurando AddressInfo como Owned Type
        builder.OwnsOne(u => u.Address, address =>
        {
            address.Property(a => a.City)
                   .HasColumnName("city")
                   .HasColumnType("text");

            address.Property(a => a.Street)
                   .HasColumnName("street")
                   .HasColumnType("text");

            address.Property(a => a.Number)
                   .HasColumnName("number")
                   .HasColumnType("int");

            address.Property(a => a.Zipcode)
                   .HasColumnName("zipcode")
                   .HasColumnType("text");

            address.OwnsOne(a => a.Geolocation, geo =>
            {
                geo.Property(g => g.Latitude)
                   .HasColumnName("geo_lat")
                   .HasColumnType("text");

                geo.Property(g => g.Longitude)
                   .HasColumnName("geo_long")
                   .HasColumnType("text");
            });
        });
    }
}
