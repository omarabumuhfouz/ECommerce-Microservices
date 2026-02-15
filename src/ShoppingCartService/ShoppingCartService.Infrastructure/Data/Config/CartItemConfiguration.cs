using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCartService.Domain.CartManagement;

namespace ShoppingCartService.Infrastructure.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
            .ValueGeneratedOnAdd();

            builder.Property(ci => ci.CartId)
                   .IsRequired();

            builder.Property(ci => ci.ProductId)
                   .IsRequired();

            // Map Quantity (record class) to int column
            builder.OwnsOne(
                ci => ci.Quantity,
                q =>
                {
                    q.Property(qv => qv.Value)
                     .HasColumnName("Quantity")
                     .IsRequired();
                    q.ToTable(s => s.HasCheckConstraint("CK_CartItem_Quantity", "[Quantity] BETWEEN 0 AND 100000"));
                });

            // Map UnitPrice (record class) to decimal column
            builder.OwnsOne(
                ci => ci.UnitPrice,
                up =>
                {
                    up.Property(upv => upv.Value)
                      .HasColumnName("UnitPrice")
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();
                    up.ToTable(s => s.HasCheckConstraint("CK_CartItem_UnitPrice", "[UnitPrice] BETWEEN 0.01 AND 10000.00"));
                });

            // Map Discount (record class) to decimal column
            builder.OwnsOne(
                ci => ci.Discount,
                d =>
                {
                    d.Property(dv => dv.Value)
                     .HasColumnName("Discount")
                     .HasColumnType("decimal(18,2)")
                     .IsRequired(true);
                    d.ToTable(s => s.HasCheckConstraint("CK_CartItem_Discount", "[Discount] BETWEEN 0.00 AND 1000.00"));
                });

            builder.Property(ci => ci.CreatedAt)
                   .IsRequired();

            builder.Property(ci => ci.UpdatedAt)
                   .IsRequired();

            // Ignore computed property
            builder.Ignore(ci => ci.TotalPrice);
        }
    }
}
