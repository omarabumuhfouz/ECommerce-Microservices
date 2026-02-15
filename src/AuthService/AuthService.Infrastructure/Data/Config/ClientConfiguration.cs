using AuthService.Domain.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Data.Config;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        // Table name (optional)
        builder.ToTable("Clients");

        // Primary Key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.ClientId)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(c => c.ClientURL)
               .IsRequired()
               .HasMaxLength(200);
    }
}
