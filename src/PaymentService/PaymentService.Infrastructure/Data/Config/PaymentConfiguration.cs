using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain.Payments;
using PaymentService.Domain.Payments.Enums;
using PaymentService.Domain.Payments.ValueObjects;

namespace PaymentService.Infrastructure.Config
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id)
                .IsClustered(false);

            builder.Property(p => p.CreatedOnUtc)
            .IsRequired();

            builder.HasIndex(p => p.CreatedOnUtc)
            .IsClustered();

            builder.Property(p => p.Id)

                .ValueGeneratedNever();

            builder.Property(p => p.OrderId)
                .IsRequired();


            builder.Property(p => p.TransactionId)
                .HasMaxLength(50);

            builder.Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");


            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(p => p.Method)
            .HasConversion(
                     method => method.Value,
                     value => PaymentMethod.FromValue(value))
            .IsRequired();


            builder.Property(p => p.Status)
                    .HasConversion(s => s.Value, v => PaymentStatus.FromValue(v))
                    .IsRequired();

            builder.Property(p => p.Amount)
                        .HasConversion(
                            money => money.Value,                         // To Database
                            value => Money.Create(value).Value)           // From Database
                        .HasPrecision(18, 2)
                        .IsRequired();

            // Also ensure your Private List of Refunds is mapped here
            builder.HasMany(p => p.Refunds)
                .WithOne(r => r.Payment)
                .HasForeignKey(r => r.PaymentId);
            
        // If you are using a backing field for the collection:
        var navigation = builder.Metadata.FindNavigation(nameof(Payment.Refunds));
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
            
        }
    }
}