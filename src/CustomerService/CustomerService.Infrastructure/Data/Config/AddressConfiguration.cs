using CustomerService.Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerService.Infrastructure.Data.Config;

public sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedNever();

        // Relationships
        builder.HasOne(a => a.Customer)
            .WithMany(c => c.Addresses)
            .HasForeignKey(a => a.CustomerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Properties
        builder.Property(a => a.CustomerId)
            .IsRequired();

        builder.Property(a => a.AddressLine1)
            .HasMaxLength(AddressConstants.ADDRESS_LINE1_MAX_LENGTH)
            .IsRequired();

        builder.Property(a => a.AddressLine2)
            .HasMaxLength(AddressConstants.ADDRESS_LINE2_MAX_LENGTH)
            .IsRequired();

        builder.Property(a => a.City)
            .HasMaxLength(AddressConstants.CITY_MAX_LENGTH)
            .IsRequired();

        builder.Property(a => a.State)
            .HasMaxLength(AddressConstants.STATE_MAX_LENGTH)
            .IsRequired();

        builder.Property(a => a.PostalCode)
            .HasMaxLength(AddressConstants.POSTAL_CODE_MAX_LENGTH)
            .IsRequired();

        builder.Property(a => a.Country)
            .HasMaxLength(AddressConstants.COUNTRY_MAX_LENGTH)
            .IsRequired();

        builder.Property(a => a.IsDefault)
            .IsRequired();

        // Navigation configuration (read-only access)
        builder.Navigation(a => a.Customer)
            .UsePropertyAccessMode(PropertyAccessMode.Property);
    }
}