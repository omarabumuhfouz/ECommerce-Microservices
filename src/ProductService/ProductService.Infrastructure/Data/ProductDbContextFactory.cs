using Microsoft.EntityFrameworkCore.Design;

namespace ProductService.Infrastructure.Data;

public class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();

        // Replace with your actual connection string
        optionsBuilder.UseSqlServer
        ("Server=localhost,9111;Database=ProductService;User Id=sa;Password=Password12345!;TrustServerCertificate=True;Encrypt=True;");
        return new ProductDbContext(optionsBuilder.Options);
    }
}
