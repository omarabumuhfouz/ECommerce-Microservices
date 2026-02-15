using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Services;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Interceptors;
using OrderService.Infrastructure.Services;
using SharedKernel;
using SharedKernel.Abstractions.Messaging;
using Quartz;
using OrderService.Infrastructure.BackgroundJobs;
using SharedKernel.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Hybrid;
using OrderService.Domain.Constants;

namespace OrderService.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
                .AddBusinessServices()
                .AddQuartzInfrastructure()
                .AddDatabase(configuration)
                .AddCustomerGrpcClient(configuration)
                .AddProductGrpcClient(configuration)
                .AddCachingService(configuration);
    }


    private static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        // // Change this line in AddBusinessServices:
        // services.Scan(selector => selector
        //     .FromAssemblies(typeof(OrderService.Application.AssemblyReference).Assembly) // Use your Application project's AssemblyReference
        //     .AddClasses(filter => filter.AssignableTo(typeof(IDomainEventHandler<>)))
        //     .AsImplementedInterfaces()
        //     .WithTransientLifetime());

        services.AddScoped<ICustomerService, GrpcCustomerService>();
        services.AddScoped<IProductService, GrpcProductService>();
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();
        // services.Decorate(typeof(IDomainEventHandler<>), typeof(IdempotentDomainEventHandler<>));

        return services;
    }

    private static IServiceCollection AddQuartzInfrastructure(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(opts => opts.WithIdentity(jobKey))
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(100)
                                        .RepeatForever()));

            configure.AddJob<OrderExpirationJob>(opts => opts.StoreDurably());

        });

        services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

        services.AddQuartzHostedService();

        services.AddScoped<IOrderExpirationScheduler, OrderExpirationScheduler>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
{
    services.AddDbContext<OrderDbContext>((sp, options) =>
    {
        // 1. Resolve the interceptor from the service provider
        var outboxInterceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
        var auditInterceptor = sp.GetRequiredService<UpdateAuditableEntitiesInterceptor>();

        // 2. Attach them to the DbContext
        options.UseSqlServer(configuration.GetConnectionString("OrderConnection"),
            sqlOptions => sqlOptions.EnableRetryOnFailure())
            .AddInterceptors(outboxInterceptor, auditInterceptor) // THIS IS THE MISSING PART
            .LogTo(Console.WriteLine, LogLevel.Information);
    });

    services.AddScoped<Microsoft.EntityFrameworkCore.DbContext>(provider =>
            provider.GetRequiredService<OrderDbContext>());

    return services;
}


    public static IServiceCollection AddCachingService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "OrderService";
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
