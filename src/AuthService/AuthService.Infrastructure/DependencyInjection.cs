using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Identity;
using AuthService.Infrastructure.Services;
using AuthService.Infrastructure.Backgrounds;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Hybrid;
using AuthService.Domain.Constants;
using AuthService.Application.Interfaces;

namespace AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
                .AddDataBase(configuration)
                .AddBusinessServices()
                .AddBackgroundServices()
                .AddCachingService(configuration);

        return services;
    }

    private static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("AuthServiceDb"),
                        options => options.EnableRetryOnFailure())
                        .LogTo(Console.WriteLine, LogLevel.Information));

        // 2. ADD THIS LINE TO FIX THE CRASH:
        services.AddScoped<Microsoft.EntityFrameworkCore.DbContext>(provider =>
                provider.GetRequiredService<AppDbContext>());

        return services;
    }


    private static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ISigningKeyRepository, SigningKeyRepository>();

        return services;
    }

    private static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<KeyRotationService>();

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