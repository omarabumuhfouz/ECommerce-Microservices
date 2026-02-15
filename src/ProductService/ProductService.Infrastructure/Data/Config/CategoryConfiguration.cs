using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.CategoryManagement;
using ProductService.Domain.Constants;

namespace ProductService.Infrastructure.Config;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Table name (optional, EF will pluralize by default)
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        // Properties
        builder.OwnsOne(c => c.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value)
            .HasColumnName("Name")
            .HasMaxLength(CategoryConstants.NAME_MAX_LENGTH);
        });

        builder.Property(c => c.Description)
            .HasMaxLength(CategoryConstants.DESCRIPTION_MAX_LENGTH);

        builder.Property(c => c.IsActive)
            .IsRequired();

    }
}
