using FeedbackService.Domain.Feedbacks;
using FeedbackService.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace FeedbackService.Infrastructure.Data;

public class FeedbackDbContext : DbContext
{
    public FeedbackDbContext(DbContextOptions<FeedbackDbContext> options)
        : base(options)
    {
    }

    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<InboxMessage> InboxMessages { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FeedbackDbContext).Assembly);

    }
}