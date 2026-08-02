using System.Globalization;
using Api.Models.Admins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Api.Data.Config;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    private static readonly ValueConverter<DateTime, string> DateTimeConverter = new(
        v => v.ToUniversalTime().ToString("O"),
        v => DateTime.Parse(v, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
    );

    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("Admins");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.FullName)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(a => a.Cpf)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(a => a.UserType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(a => a.Birthdate)
            .IsRequired()
            .HasConversion(DateTimeConverter)
            .HasMaxLength(30);

        builder.Property(a => a.CreatedAt)
            .IsRequired()
            .HasConversion(DateTimeConverter)
            .HasMaxLength(30);

        builder.Property(a => a.Approved)
            .IsRequired();

        builder.HasIndex(a => a.Cpf).IsUnique();
        builder.HasIndex(a => a.Email).IsUnique();
    }
}
