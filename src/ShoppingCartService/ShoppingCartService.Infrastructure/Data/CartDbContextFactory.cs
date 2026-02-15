using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ShoppingCartService.Infrastructure.Data;

namespace CustomerService.Infrastructure.Data
{
    public class CartDbContextFactory : IDesignTimeDbContextFactory<CartDbContext>
    {
        public CartDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CartDbContext>();

            // Replace with your actual connection string
            optionsBuilder.UseSqlServer
            ("Server=localhost,9111;Database=CartService;User Id=sa;Password=Password12345!;TrustServerCertificate=True;");

            return new CartDbContext(optionsBuilder.Options);
        }
    }
}
