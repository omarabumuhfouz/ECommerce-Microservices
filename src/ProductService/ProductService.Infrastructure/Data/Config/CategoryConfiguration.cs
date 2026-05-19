using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Categories;
using ProductService.Domain.Constants;

namespace ProductService.Infrastructure.Config;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Table name (optional, EF will pluralize by default)
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id)
        .IsClustered(false);

builder.HasIndex(p => p.CreatedOnUtc)
              .IsClustered(true);

              builder.Property(p => p.CreatedOnUtc)
              .IsRequired();

              builder.Property(p => p.ModifiedOnUtc)
              .IsRequired(false);


        // Properties
        builder.OwnsOne(c => c.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value)
            .HasColumnName("Name")
            .HasMaxLength(CategoryConstants.NameMaxLength);
        });

        builder.Property(c => c.Description)
            .HasMaxLength(CategoryConstants.DescriptionMaxLength);

        builder.Property(c => c.IsActive)
            .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
