namespace CustomerService.Infrastructure.Data
{
    public class CustomerDbContextFactory : IDesignTimeDbContextFactory<CustomerDbContext>
    {
        public CustomerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();

            // Replace with your actual connection string
            optionsBuilder.UseSqlServer
            ("Server=localhost,9111;Database=CustomerService;User Id=sa;Password=Password12345!;TrustServerCertificate=True;");

            return new CustomerDbContext(optionsBuilder.Options);
        }
    }
}
