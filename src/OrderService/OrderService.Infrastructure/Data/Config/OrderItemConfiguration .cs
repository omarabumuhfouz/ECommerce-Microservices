using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Orders;
using OrderService.Domain.Orders.ValueObjects;
using SharedKernel.Common;

namespace OrderService.Infrastructure.Data.Config;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(oi => oi.Id);
        builder.Property(x => x.Id)
        .ValueGeneratedOnAdd();



        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(o => o.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(oi => oi.ProductName)
            .IsRequired()
            .HasMaxLength(200); 

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.DiscountPercentage)
            .IsRequired()
            .HasColumnType("decimal(5, 2)");

        builder.OwnsOne(oi => oi.UnitPrice, ownedBuilder =>
        {
            ownedBuilder.Property(m => m.Value)
                .HasColumnName("UnitPrice")
                .HasColumnType("decimal(18, 2)")
                .IsRequired();
        });

        builder.Ignore(oi => oi.Discount);
        builder.Ignore(oi => oi.TotalPrice);
    }
}
