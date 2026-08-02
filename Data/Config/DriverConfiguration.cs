using System.Globalization;
using Api.Models.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Api.Data.Config;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    private static readonly ValueConverter<DateTime, string> DateTimeConverter = new(
        v => v.ToUniversalTime().ToString("O"),
        v => DateTime.Parse(v, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
    );

    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable("Drivers");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.FullName)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(d => d.Cpf)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(d => d.Email)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(d => d.UserType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(d => d.Birthdate)
            .IsRequired()
            .HasConversion(DateTimeConverter)
            .HasMaxLength(30);

        builder.Property(d => d.CreatedAt)
            .IsRequired()
            .HasConversion(DateTimeConverter)
            .HasMaxLength(30);

        builder.Property(d => d.Gender)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(d => d.Approved)
            .IsRequired();

        builder.HasIndex(d => d.Cpf).IsUnique();
        builder.HasIndex(d => d.Email).IsUnique();
    }
}
