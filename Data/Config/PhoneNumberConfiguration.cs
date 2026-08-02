using Api.Models.Info;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Config;

public class PhoneNumberConfiguration : IEntityTypeConfiguration<PhoneNumber>
{
    public void Configure(EntityTypeBuilder<PhoneNumber> builder)
    {
        builder.ToTable("PhoneNumbers");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.Property(p => p.UserType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(p => p.Number)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.AreaCode)
            .IsRequired()
            .HasMaxLength(5);

        builder.Property(p => p.CountryCode)
            .IsRequired()
            .HasMaxLength(5);

        builder.Property(p => p.Type)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(p => new { p.UserId, p.UserType });
    }
}
