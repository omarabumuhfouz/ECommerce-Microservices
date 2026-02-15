using Microsoft.EntityFrameworkCore.Design;

namespace PaymentService.Infrastructure.Data;

public class PaymentDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
{
    PaymentDbContext IDesignTimeDbContextFactory<PaymentDbContext>.CreateDbContext(string[] args)
    {
        const string hardCodedConnectionString = 
            "Server=localhost,9111;Database=PaymentService;User Id=sa;Password=Password12345!;TrustServerCertificate=True;Encrypt=True;";

        var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();
        optionsBuilder.UseSqlServer(hardCodedConnectionString);

        return new PaymentDbContext(optionsBuilder.Options);
    }
}