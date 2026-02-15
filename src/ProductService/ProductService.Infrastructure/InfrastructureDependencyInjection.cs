using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductService.Application.Services;
using ProductService.Domain.Constants;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Services;

namespace ProductService.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddProductInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDatabase(configuration)
            .AddCachingService(configuration)
            .AddBusinessServices();



        return services;
    }

    

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ProductDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ProductConnection"),
            options => options.EnableRetryOnFailure())
            .LogTo(Console.WriteLine, LogLevel.Information));


        // 2. ADD THIS LINE TO FIX THE CRASH:
        services.AddScoped<Microsoft.EntityFrameworkCore.DbContext>(provider =>
                provider.GetRequiredService<ProductDbContext>());

        return services;
    }


    public static IServiceCollection AddCachingService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "ProductService:";
        });


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

    private static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IIdempotencyService, IdempotencyService>();

        return services;
    }


}
