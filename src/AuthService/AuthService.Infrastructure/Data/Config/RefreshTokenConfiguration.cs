using AuthService.Domain.RefreshTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Data.Config;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // Table name (optional)
        builder.ToTable("RefreshTokens");

        // Primary Key
        builder.HasKey(rt => rt.Id);

        // Unique Index
        builder.HasIndex(rt => rt.Token)
               .IsUnique()
               .HasDatabaseName("IX_Token_Unique");

        // Properties
        builder.Property(rt => rt.Token)
               .IsRequired();

        builder.Property(rt => rt.UserId)
               .IsRequired();

        builder.Property(rt => rt.ClientId)
               .IsRequired();

        builder.Property(rt => rt.ExpiresAt)
               .IsRequired();

        builder.Property(rt => rt.IsRevoked)
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(rt => rt.CreatedAt)
               .IsRequired()
               .HasDefaultValueSql("GETUTCDATE()"); // for SQL Server

        builder.Property(rt => rt.RevokedAt)
               .IsRequired(false);

    }
}
