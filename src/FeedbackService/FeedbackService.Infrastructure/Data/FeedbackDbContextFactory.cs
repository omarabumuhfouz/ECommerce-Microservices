using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FeedbackService.Infrastructure.Data;

public class FeedbackDbContextFactory : IDesignTimeDbContextFactory<FeedbackDbContext>
{
    public FeedbackDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FeedbackDbContext>();
        
        // This connection string is only used for local migration generation
        optionsBuilder.UseSqlServer("Server=localhost,9111;Database=FeedbackService;User Id=sa;Password=Password12345!;TrustServerCertificate=True;Encrypt=True;");

        return new FeedbackDbContext(optionsBuilder.Options);
    }
}