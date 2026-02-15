using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCartService.Domain.CartManagement;

namespace ShoppingCartService.Infrastructure.Config
{
       public class CartConfiguration : IEntityTypeConfiguration<Cart>
       {
              public void Configure(EntityTypeBuilder<Cart> builder)
              {
                     builder.HasKey(c => c.Id);

                     builder.Property(c => c.CustomerId)
                            .IsRequired();

                     builder.Property(c => c.IsCheckedOut)
                            .IsRequired()
                            .HasDefaultValue(false);

                     builder.Property(c => c.CreatedAt)
                            .IsRequired();

                     builder.Property(c => c.UpdatedAt)
                            .IsRequired();

                     builder.HasMany(c => c.CartItems)
                            .WithOne(ci => ci.Cart)
                            .HasForeignKey(ci => ci.CartId);

                     // Tell EF to use the field, not the property
                     builder
                         .Navigation(c => c.CartItems)
                         .UsePropertyAccessMode(PropertyAccessMode.Field);

                     builder.ToTable("Carts");

              }
       }
}
