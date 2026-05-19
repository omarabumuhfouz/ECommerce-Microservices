using InventoryService.Domain.InventoryItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryService.Infrastructure.Data.Config;

public sealed class StockReservationConfiguration : IEntityTypeConfiguration<StockReservation>
{
    public void Configure(EntityTypeBuilder<StockReservation> builder)
    {
        builder.ToTable("StockReservations");

        builder.HasKey(x => x.Id)
            .IsClustered(false);

        builder.Property(x => x.OrderId)
            .IsRequired();

        builder.Property(x => x.ReservedAtUtc)
            .IsRequired();

        builder.HasIndex(x => x.ReservedAtUtc)
                   .IsClustered(true);

        builder.OwnsOne(x => x.Quantity, qtyBuilder =>
        {
            qtyBuilder.Property(q => q.Value)
                .HasColumnName("Quantity")
                .IsRequired();
        });
    }
}