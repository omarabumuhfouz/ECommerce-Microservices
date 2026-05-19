using CustomerService.Domain.Constants;
using CustomerService.Domain.Customers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerService.Infrastructure.Data.Config;
   
public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id)
            .IsClustered(false);


        builder.Property(c => c.Id)
            .ValueGeneratedNever();

builder.HasIndex(c => c.CreatedOnUtc)
            .IsClustered(true);

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.OwnsOne(c => c.FullName, fn =>
        {
            fn.Property(f => f.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(CustomerConstants.MAX_FIRST_NAME_LENGTH)
                .IsRequired();

            fn.Property(f => f.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(CustomerConstants.MAX_LAST_NAME_LENGTH)
                .IsRequired();
        });

        builder.OwnsOne(c => c.PhoneNumber, pn =>
        {
            pn.Property(p => p.Value)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(CustomerConstants.MAX_PHONE_NUMBER_LENGTH)
                .IsRequired();
        });

        builder.HasMany(c => c.Addresses)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);



        builder.Navigation(c => c.Addresses)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}