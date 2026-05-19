using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;
using Quartz;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Hybrid;
using InventoryService.Infrastructure.Interceptors;
using InventoryService.Application.Services;
using InventoryService.Infrastructure.Services;
using InventoryService.Infrastructure.Data;
using MassTransit;
using InventoryService.Domain.Constants;
using InventoryService.Infrastructure.BackgroundJobs;

namespace InventoryService.Infrastructure;

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
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();

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


        });

        services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

        services.AddQuartzHostedService();


        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
{
    services.AddDbContext<InventoryDbContext>((sp, options) =>
    {
        // 1. Resolve the interceptor from the service provider
        var outboxInterceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
        var auditInterceptor = sp.GetRequiredService<UpdateAuditableEntitiesInterceptor>();

        // 2. Attach them to the DbContext
        options.UseSqlServer(configuration.GetConnectionString("InventoryConnection"),
            sqlOptions => sqlOptions.EnableRetryOnFailure())
            .AddInterceptors(outboxInterceptor, auditInterceptor) // THIS IS THE MISSING PART
            .LogTo(Console.WriteLine, LogLevel.Information);
    });

    services.AddScoped<Microsoft.EntityFrameworkCore.DbContext>(provider =>
            provider.GetRequiredService<InventoryDbContext>());

    return services;
}


    public static IServiceCollection AddCachingService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "InventoryService";
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

private static IServiceCollection AddMassTransitService(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            // x.AddConsumer<PaymentCompletedConsumer>();
            // x.AddConsumer<PaymentFailedConsumer>();
            // x.AddConsumer<PaymentFailedConsumer>();
            // x.AddConsumer<PaymentRefundedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("", e =>
                {
                    // e.ConfigureConsumer<PaymentCompletedConsumer>(context);
                    // e.ConfigureConsumer<PaymentFailedConsumer>(context);
                    // e.ConfigureConsumer<PaymentRefundedConsumer>(context);
                });
            });
        });
        return services;
    }


}
