using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Constants;
using ProductService.Domain.Tags;

namespace ProductService.Infrastructure.Data.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id)
        .IsClustered(false);

        builder.HasIndex(p => p.CreatedOnUtc)
                      .IsClustered(true);

              builder.Property(p => p.CreatedOnUtc)
              .IsRequired();

              builder.Property(p => p.ModifiedOnUtc)
              .IsRequired(false);


        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(TagConstants.TagNameMaxLength);

        builder.HasIndex(t => t.Name)
            .IsUnique();

        builder.HasMany(t => t.Products)
            .WithMany(p => p.Tags);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}