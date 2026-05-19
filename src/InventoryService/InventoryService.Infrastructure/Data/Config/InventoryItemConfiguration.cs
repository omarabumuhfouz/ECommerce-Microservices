using InventoryService.Domain.InventoryItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryService.Infrastructure.Data.Config;

public sealed class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
       public void Configure(EntityTypeBuilder<InventoryItem> builder)
       {
              builder.ToTable("InventoryItems");

              builder.HasKey(x => x.Id)
              .IsClustered(false);

              builder.Property(x => x.ProductId)
                  .IsRequired();

              builder.Property(x => x.LowStockThreshold)
                  .IsRequired();

              builder.Property(x => x.CreatedOnUtc)
                  .IsRequired();

              builder.HasIndex(x => x.CreatedOnUtc)
                         .IsClustered(true);

              builder.Property(x => x.ModifiedOnUtc)
                  .IsRequired(false);

              builder.OwnsOne(x => x.AvailableStock, stockBuilder =>
              {
                     stockBuilder.Property(q => q.Value)
                   .HasColumnName("AvailableStock")
                   .IsRequired();
              });

              builder.Ignore(x => x.CurrentAvailableStock);
              builder.Ignore(x => x.StockOnHand);
              builder.Ignore(x => x.ReservedStock);
              builder.Ignore(x => x.IsInStock);
              builder.Ignore(x => x.Deficit);

              builder.HasMany(x => x.Reservations)
                  .WithOne() 
                  .HasForeignKey("InventoryItemId") 
                  .OnDelete(DeleteBehavior.Cascade);

              builder.Metadata.FindNavigation(nameof(InventoryItem.Reservations))!
                  .SetPropertyAccessMode(PropertyAccessMode.Field);
       }
}