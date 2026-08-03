using Api.Models.Info;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Config;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.UserId)
            .IsRequired();

        builder.Property(a => a.UserType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(a => a.Street)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(a => a.Number)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.ZipCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.Neighborhood)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.State)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Complement)
            .HasMaxLength(250);

        builder.Property(a => a.Line1)
            .HasMaxLength(250);

        builder.Property(a => a.Line2)
            .HasMaxLength(250);

        builder.HasIndex(a => new { a.UserId, a.UserType }).IsUnique();
    }
}
