using System.Reflection;
using AuthService.Domain.Clients;
using AuthService.Domain.RefreshTokens;
using AuthService.Domain.SigningKey;
using AuthService.Domain.Users;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Client> Clients { get; set; }
    public DbSet<SigningKey> SigningKeys { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            base.OnModelCreating(modelBuilder);
            
            var client1Id = Guid.Parse("b2d3c4e5-f678-4d12-a345-9b0c12345678");
            var client2Id = Guid.Parse("c3e4f5a6-b789-4e23-b456-0d1e23456789");

            modelBuilder.Entity<Client>().HasData();

            modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

// dotnet ef migrations add AddOutboxTables
// dotnet ef database update


        }
    }
}