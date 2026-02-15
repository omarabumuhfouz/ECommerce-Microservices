using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharedKernel;
using ShoppingCartService.Application.Services;
using ShoppingCartService.Domain.Constants;
using ShoppingCartService.Infrastructure.Data;
using ShoppingCartService.Infrastructure.Services;

namespace ShoppingCartService.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddCartsInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddCustomerGrpcClient(configuration)
            .AddDbContextService(configuration)
            .AddBusinessServices()
            .AddCustomerGrpcClient(configuration)
            .AddProductGrpcClient(configuration)
            .AddCachingService(configuration);

        return services;
    }

    private static IServiceCollection AddDbContextService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CartDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CartConnection"),
            options => options.EnableRetryOnFailure())
            .LogTo(Console.WriteLine, LogLevel.Information));


        services.AddScoped<Microsoft.EntityFrameworkCore.DbContext>(provider =>
                provider.GetRequiredService<CartDbContext>());

        return services;
    }

    private static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<ICartMapper, CartMapper>();
        services.AddScoped<IProductService, GrpcProductService>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();


        return services;
    }

    private static IServiceCollection AddCustomerGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        // Make sure the key matches your appsettings.json
        var customerServiceUrl = configuration["ServiceUrls:CustomerService"];

        if (string.IsNullOrEmpty(customerServiceUrl))
        {
            throw new InvalidOperationException("Customer Service URL is missing in configuration.");
        }

        services.AddGrpcClient<Contracts.Customer.CustomerProtoService.CustomerProtoServiceClient>(options =>
        {
            options.Address = new Uri(customerServiceUrl);
        });

        return services;
    }
    private static IServiceCollection AddProductGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        // Make sure the key matches your appsettings.json
        var customerServiceUrl = configuration["ServiceUrls:ProductService"];

        if (string.IsNullOrEmpty(customerServiceUrl))
        {
            throw new InvalidOperationException("Product Service URL is missing in configuration.");
        }

        services.AddGrpcClient<Contracts.Product.ProductProtoService.ProductProtoServiceClient>(options =>
        {
            options.Address = new Uri(customerServiceUrl);
        });

        return services;
    }

        public static IServiceCollection AddCachingService(this IServiceCollection services, IConfiguration configuration)
{
    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = configuration.GetConnectionString("Redis");
        options.InstanceName = "CartService:";
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
