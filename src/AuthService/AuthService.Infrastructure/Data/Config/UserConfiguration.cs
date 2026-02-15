using AuthService.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Data.Config;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table name (optional if default naming is OK)
        builder.ToTable("Users");

        // Primary Key
        builder.HasKey(u => u.Id);

        // Index for Email, unique
        builder.HasIndex(u => u.Email)
               .IsUnique()
               .HasDatabaseName("IX_Unique_Email");

        // Properties
        builder.Property(u => u.Email)
               .IsRequired();

        builder.Property(u => u.PasswordHash)
               .IsRequired();

        builder.Property(u => u.Roles)
               .IsRequired();

        builder.Property(u => u.CreatedAt)
               .IsRequired()
               .HasDefaultValueSql("GETUTCDATE()"); // for SQL Server
    }
}
