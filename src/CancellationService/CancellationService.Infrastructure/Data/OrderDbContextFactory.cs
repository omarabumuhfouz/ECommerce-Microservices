using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CancellationService.Infrastructure.Data;

public class CancellationDbContextFactory : IDesignTimeDbContextFactory<CancellationDbContext>
{
    CancellationDbContext IDesignTimeDbContextFactory<CancellationDbContext>.CreateDbContext(string[] args)
    {
        const string hardCodedConnectionString = 
            "Server=localhost,9111;Database=CancellationService;User Id=sa;Password=Password12345!;TrustServerCertificate=True;Encrypt=True;";

        var optionsBuilder = new DbContextOptionsBuilder<CancellationDbContext>();
        optionsBuilder.UseSqlServer(hardCodedConnectionString);

        return new CancellationDbContext(optionsBuilder.Options);
    }
}