using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Constants;
using ProductService.Domain.ProductManagement;

namespace ProductService.Infrastructure.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
       public void Configure(EntityTypeBuilder<Product> builder)
       {
              builder.ToTable("Products");

              builder.HasKey(p => p.Id);

              builder.Property(p => p.Name)
                     .IsRequired()
                     .HasMaxLength(ProductConstants.NAME_MAX_LENGTH);

              builder.Property(p => p.Description)
                     .IsRequired()
                     .HasMaxLength(ProductConstants.DESCRIPTION_MAX_LENGTH);

              builder.Property(p => p.StockQuantity)
                     .IsRequired();

              builder.Property(p => p.IsAvailable)
                     .IsRequired();

              builder.OwnsOne(p => p.Price, priceBuilder =>
                           {
                                  priceBuilder.Property(m => m.Amount)
                  .HasColumnName("PriceAmount")
                  .HasColumnType("decimal(18,2)");

                                  priceBuilder.Property(m => m.Currency)
                  .HasColumnName("PriceCurrency")
                  .IsRequired()
                  .HasMaxLength(3);
                           });

              builder.OwnsOne(p => p.Discount);
              builder.OwnsOne(p => p.MainImage);

              builder.OwnsMany(p => p.RelatedImages, imageBuilder =>
                      {
                             imageBuilder.ToTable("ProductRelatedImages");
                             imageBuilder.WithOwner().HasForeignKey("ProductId");
                             imageBuilder.Property(i => i.Url).IsRequired();
                             imageBuilder.Property(i => i.AltText);

                             imageBuilder.Property<Guid>("Id");

                             imageBuilder.HasKey("Id", "ProductId");

                             imageBuilder.Property<Guid>("Id").ValueGeneratedOnAdd();
                      });

              builder.OwnsMany(p => p.Features, featureBuilder =>
              {
                     featureBuilder.ToTable("ProductFeatures");
                     featureBuilder.WithOwner().HasForeignKey("ProductId");
                     featureBuilder.Property(f => f.Name).IsRequired();
                     featureBuilder.Property(f => f.Value).IsRequired();

                     featureBuilder.Property<Guid>("Id");

                     featureBuilder.HasKey("Id", "ProductId");

                     featureBuilder.Property<Guid>("Id").ValueGeneratedOnAdd();
              });

              // One-to-Many: Product has one Category

              // Many-to-Many: Product has many Tags
              builder.HasMany(p => p.Tags)
                  .WithMany(t => t.Products)
                  .UsingEntity(j => j.ToTable("ProductTags")); // Creates the join table

       }
}