using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrderService.Infrastructure.Data;

public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
{
    OrderDbContext IDesignTimeDbContextFactory<OrderDbContext>.CreateDbContext(string[] args)
    {
        const string hardCodedConnectionString = 
            "Server=localhost,9111;Database=OrderService;User Id=sa;Password=Password12345!;TrustServerCertificate=True;Encrypt=True;";

        var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();
        optionsBuilder.UseSqlServer(hardCodedConnectionString);

        return new OrderDbContext(optionsBuilder.Options);
    }
}