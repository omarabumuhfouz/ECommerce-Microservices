using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain.Payments;
using PaymentService.Domain.Payments.Enums;
using PaymentService.Domain.Payments.ValueObjects;

namespace PaymentService.Infrastructure.Data.Config;

public class RefundConfiguration : IEntityTypeConfiguration<Refund>
{
    public void Configure(EntityTypeBuilder<Refund> builder)
    {
        builder.ToTable("Refunds");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
        .ValueGeneratedOnAdd();

        builder.HasIndex(r => r.CancellationId)
                       .IsUnique();


        builder.Property(r => r.Amount)
            .HasConversion(
                money => money.Value,
                value => Money.Create(value).Value)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(r => r.Reason)
            .HasConversion(
                reason => reason != null ? reason.Value : null,
                value => value != null ? Reason.Create(value).Value : null)
            .HasMaxLength(500); 

        builder.Property(r => r.TransactionId)
            .HasConversion(
                tx => tx != null ? tx.Value : null,
                value => value != null ? TransactionId.Create(value).Value : null)
            .HasMaxLength(100); 

        // Smart Enum Conversion
        builder.Property(r => r.Method)
            .HasConversion(
                method => method.Value,
                value => PaymentMethod.FromValue(value))
            .IsRequired();

        // Standard Enum
        builder.Property(r => r.Status)
            .HasConversion<int>()
            .IsRequired();

        // Foreign Key Relationship
        builder.HasOne(r => r.Payment)
            .WithMany(p => p.Refunds)
            .HasForeignKey(r => r.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(r => r.InitiatedAt).IsRequired();
        builder.Property(r => r.CompletedAt).IsRequired(false);
        builder.Property(r => r.ProcessedBy).IsRequired(false);
    }
}