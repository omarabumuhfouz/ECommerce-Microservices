using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InventoryService.Infrastructure.Data.Config;

public class InventoryDbContextFactory : IDesignTimeDbContextFactory<InventoryDbContext>
{
    InventoryDbContext IDesignTimeDbContextFactory<InventoryDbContext>.CreateDbContext(string[] args)
    {
        const string hardCodedConnectionString = 
            "Server=localhost,9111;Database=InventoryService;User Id=sa;Password=Password12345!;TrustServerCertificate=True;Encrypt=True;";

        var optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
        optionsBuilder.UseSqlServer(hardCodedConnectionString);

        return new InventoryDbContext(optionsBuilder.Options);
    }
}