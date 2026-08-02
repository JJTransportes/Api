using System.Globalization;
using Api.Models.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Api.Data.Config;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    private static readonly ValueConverter<DateTime, string> DateTimeConverter = new(
        v => v.ToUniversalTime().ToString("O"),
        v => DateTime.Parse(v, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
    );

    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.FullName)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(c => c.Cpf)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(c => c.UserType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(c => c.Birthdate)
            .IsRequired()
            .HasConversion(DateTimeConverter)
            .HasMaxLength(30);

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasConversion(DateTimeConverter)
            .HasMaxLength(30);

        builder.Property(c => c.Gender)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Approved)
            .IsRequired();

        builder.HasIndex(c => c.Cpf).IsUnique();
        builder.HasIndex(c => c.Email).IsUnique();
    }
}
