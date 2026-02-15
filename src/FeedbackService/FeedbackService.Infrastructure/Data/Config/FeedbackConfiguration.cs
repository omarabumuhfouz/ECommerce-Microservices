using FeedbackService.Domain.Feedbacks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FeedbackService.Infrastructure.Data.Configurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        // 1. Table and Key
        builder.ToTable("Feedbacks");
        builder.HasKey(f => f.Id);

        // 2. Simple Properties
        builder.Property(f => f.CustomerName)
            .IsRequired()
            .HasMaxLength(FeedbackConstants.MaxCustomerNameLength);

        builder.Property(f => f.ProductName)
            .IsRequired()
            .HasMaxLength(FeedbackConstants.MaxProductNameLength);

        builder.Property(f => f.CreatedAt)
            .IsRequired();

        builder.Property(f => f.UpdatedAt)
            .IsRequired();

        // 3. Mapping Value Object: Rating
        // This converts the 'Rating' record to a simple 'int' in the DB
        builder.Property(f => f.Rating)
            .HasConversion(
                rating => rating.Value,           // To Database (int)
                value => Rating.Create(value).Value // From Database (Rating)
            )
            .HasColumnName("Rating")
            .IsRequired();

        // 4. Mapping Value Object: Comment
        // This converts the 'Comment' record to a 'nvarchar(500)' in the DB
        builder.Property(f => f.Comment)
            .HasConversion(
                comment => comment.Value,          // To Database (string)
                value => Comment.Create(value).Value // From Database (Comment)
            )
            .HasColumnName("Comment")
            // Assuming FeedbackConstants.MaxCommentLength is 500
            .HasMaxLength(FeedbackConstants.MaxCommentLength) 
            .IsRequired();
    }
}