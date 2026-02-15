using AuthService.Application.Interfaces;
using AuthService.Domain.Clients;
using AuthService.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        // Ensure database is created & migrated
        await db.Database.EnsureCreatedAsync();

        // Example: Seed Clients
        if (!db.Clients.Any())
        {
            db.Clients.Add(Client.Create
            (
                "myapp",
                "Shope Website",
                "http://localhost:9090"

            ).Value);

            db.Clients.Add(Client.Create
            (
                "mobile-client",
                "Mobile Client",
                "https://mobile.app"
            ).Value);

            await db.SaveChangesAsync();
        }

        // Example: Seed an Admin User
        if (!db.Users.Any())
        {
            var admin = User.Create(
                "admin@gmail.com",
                passwordService.HashPassword("123456"), // hash password
                new List<UserRole> { UserRole.Admin }
            );

            var customer = User.Create(
                "customer@gmail.com",
                passwordService.HashPassword("123456"),
                new List<UserRole> { UserRole.Customer }
            );

            var mixUser = User.Create(
                    "omar@gmail.com",
                    passwordService.HashPassword("123456"),
                    new List<UserRole> { UserRole.Customer, UserRole.Admin }
            );

            db.Users.AddRange(admin.Value, customer.Value);
            await db.SaveChangesAsync();
        }
    }


}
