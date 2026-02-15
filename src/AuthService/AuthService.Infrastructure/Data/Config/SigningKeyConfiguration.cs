using AuthService.Domain.SigningKey;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Data.Config;

public class SigningKeyConfiguration : IEntityTypeConfiguration<SigningKey>
{
    public void Configure(EntityTypeBuilder<SigningKey> builder)
    {
        // Table name (optional)
        builder.ToTable("SigningKeys");

        // Primary Key
        builder.HasKey(sk => sk.Id);

        // Properties
        builder.Property(sk => sk.KeyId)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(sk => sk.PrivateKey)
               .IsRequired();

        builder.Property(sk => sk.PublicKey)
               .IsRequired();

        builder.Property(sk => sk.IsActive)
               .IsRequired();

        builder.Property(sk => sk.CreatedAt)
               .IsRequired()
               .HasDefaultValueSql("GETUTCDATE()"); // optional default

        builder.Property(sk => sk.ExpiresAt)
               .IsRequired();
    }
}
