using FeedbackService.Domain.Feedbacks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FeedbackService.Infrastructure.Data;

public class FeedbackDataSeeder
{
    private readonly FeedbackDbContext _context;
    private readonly ILogger<FeedbackDataSeeder> _logger;

    public FeedbackDataSeeder(FeedbackDbContext context, ILogger<FeedbackDataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public  async Task SeedAsync()
    {
        // 1. Ensure we don't double-seed
        if (await _context.Feedbacks.AnyAsync())
        {
            _logger.LogInformation("Feedback database already seeded. Skipping.");
            return;
        }

        _logger.LogInformation("Seeding Feedback data...");

        var feedbacks = new List<Feedback>();

        // 2. Define some raw seed data
        var seedData = new[]
        {
            (CustId: Guid.NewGuid(), ProdId: Guid.NewGuid(), CustName: "John Doe", ProdName: "Laptop Pro", Rating: 5, Comment: "Amazing performance! Best laptop I've owned."),
            (CustId: Guid.NewGuid(), ProdId: Guid.NewGuid(), CustName: "Jane Smith", ProdName: "Ergo Mouse", Rating: 4, Comment: "Very comfortable, but battery life could be better."),
            (CustId: Guid.NewGuid(), ProdId: Guid.NewGuid(), CustName: "Omar Ahmed", ProdName: "Gaming Monitor", Rating: 5, Comment: "Crystal clear quality. Perfect for high-refresh gaming."),
            (CustId: Guid.NewGuid(), ProdId: Guid.NewGuid(), CustName: "Sara Wilson", ProdName: "Mechanical Keyboard", Rating: 3, Comment: "A bit too loud for my office environment, but builds well."),
            (CustId: Guid.NewGuid(), ProdId: Guid.NewGuid(), CustName: "Michael Scott", ProdName: "Coffee Mug", Rating: 2, Comment: "It leaks. Not satisfied.")
        };

        // 3. Use the Domain Factory to create entities
        foreach (var data in seedData)
        {
            var result = Feedback.Create(
                data.CustId,
                data.ProdId,
                data.CustName,
                data.ProdName,
                data.Rating,
                data.Comment);

            if (result.IsSuccess)
            {
                feedbacks.Add(result.Value);
            }
            else
            {
                _logger.LogWarning("Failed to create seed feedback for {Customer}: {Error}", 
                    data.CustName, result.TopError.Message);
            }
        }

        // 4. Save to Database
        await _context.Feedbacks.AddRangeAsync(feedbacks);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Successfully seeded {Count} Feedback records.", feedbacks.Count);
    }
}