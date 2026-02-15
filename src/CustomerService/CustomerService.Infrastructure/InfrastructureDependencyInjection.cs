using CustomerService.Domain.Constants;
using CustomerService.Infrastructure.Data;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CustomerService.Infrastructure
{
    /// <summary>
    /// Registers infrastructure-level services like DbContext and repositories.
    /// </summary>
    public static class InfrastructureDependencyInjection
    {
        /// <summary>
        /// Adds EF Core, repositories, and infrastructure services to the DI container.
        /// </summary>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                    .AddDatabase(configuration)
                    .AddCachingService(configuration);

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CustomerDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CustomerConnection"),
                options => options.EnableRetryOnFailure())
                .LogTo(Console.WriteLine, LogLevel.Information));


        // 2. ADD THIS LINE TO FIX THE CRASH:
        services.AddScoped<Microsoft.EntityFrameworkCore.DbContext>(provider => 
                provider.GetRequiredService<CustomerDbContext>());

            return services;
        }


        public static IServiceCollection AddCachingService(this IServiceCollection services, IConfiguration configuration)
{
    // 1. Configure the L2 Cache (Redis)
    // HybridCache will automatically find this and use it for distributed storage.
    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = configuration.GetConnectionString("Redis");
        options.InstanceName = "CustomerService:";
    });

    services.AddMemoryCache();

    services.AddHybridCache(options =>
    {
        options.DefaultEntryOptions = new HybridCacheEntryOptions
        {
            Expiration = CacheKeys.Expiration, 
            LocalCacheExpiration = CacheKeys.Expiration 
        };
    });

    return services;
}

    }
}
