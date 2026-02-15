using Microsoft.EntityFrameworkCore;
using ShoppingCartService.Domain.CartManagement;
using ShoppingCartService.Infrastructure.Inbox;

namespace ShoppingCartService.Infrastructure.Data
{
    public class CartDbContext : DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<InboxMessage> InboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Automatically apply all IEntityTypeConfiguration<T> classes from the same assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
