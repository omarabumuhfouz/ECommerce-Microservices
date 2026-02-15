using CancellationService.Domain.Cancellations;
using CancellationService.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace CancellationService.Infrastructure.Data
{
   public class CancellationDbContext : DbContext
   {
       public CancellationDbContext(DbContextOptions<CancellationDbContext> options)
           : base(options)
       {
       }

       public DbSet<Cancellation> Cancellations { get; set; }
        public DbSet<InboxMessage> InboxMessages { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           // Automatically apply all IEntityTypeConfiguration<> classes from this assembly
           modelBuilder.ApplyConfigurationsFromAssembly(typeof(CancellationDbContext).Assembly);

           base.OnModelCreating(modelBuilder);
       }
   }
}
