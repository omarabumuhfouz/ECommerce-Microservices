using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Orders;

namespace OrderService.Infrastructure.Data.Config;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Ignore(o => o.TotalBaseAmount);
        builder.Ignore(o => o.TotalDiscountAmount);
        builder.Ignore(o => o.TotalAmount);

        builder.OwnsOne(o => o.OrderNumber, nb =>
        {
            nb.Property(n => n.Value)
                .HasColumnName("OrderNumber")
                .HasMaxLength(30)
                .IsRequired();
        });

        builder.OwnsOne(o => o.ShippingCost, m =>
        {
            m.Property(p => p.Value)
                .HasColumnName("ShippingCost")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        builder.Property(o => o.CustomerId)
            .IsRequired();

        builder.Property(o => o.PaymentId)
            .IsRequired(false);

        builder.Property(o => o.BillingAddressId)
            .IsRequired();

        builder.Property(o => o.ShippingAddressId)
            .IsRequired();

        builder.Property(o => o.OrderDate).IsRequired();
        builder.Property(o => o.OrderStatus)
               .HasConversion<int>()
               .IsRequired();

        builder.HasMany(o => o.OrderItems)
               .WithOne()
               .HasForeignKey(oi => oi.OrderId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        builder.Metadata.FindNavigation(nameof(Order.OrderItems))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
