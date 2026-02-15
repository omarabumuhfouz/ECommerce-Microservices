using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace AuthService.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost,9111;Database=AuthService;User Id=sa;Password=Password12345!;TrustServerCertificate=True;Encrypt=True;");

        return new AppDbContext(optionsBuilder.Options);
    }
}
