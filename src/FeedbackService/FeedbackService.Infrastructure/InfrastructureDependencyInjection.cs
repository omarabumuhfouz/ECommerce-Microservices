using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Hybrid;
using FeedbackService.Application.Services;
using FeedbackService.Infrastructure.Interceptors;
using FeedbackService.Infrastructure.Services;
using FeedbackService.Infrastructure.Data;
using FeedbackService.Domain.Constants;
using SharedKernel;
using MassTransit;
using FeedbackService.Infrastructure.Messaging.Consumers;

namespace OrderService.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
                .AddBusinessServices()
                .AddDatabase(configuration)
                .AddCachingService(configuration)
                .AddOrderGrpcClient(configuration)
                .AddMassTransitService();
    }


    private static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();
        services.AddScoped<IOrderService, OrderGrpcService>();

        return services;
    }

    // private static IServiceCollection AddQuartzInfrastructure(this IServiceCollection services)
    // {
    //     services.AddQuartz(configure =>
    //     {
    //         var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

    //         configure
    //             .AddJob<ProcessOutboxMessagesJob>(opts => opts.WithIdentity(jobKey))
    //             .AddTrigger(
    //                 trigger =>
    //                     trigger.ForJob(jobKey)
    //                         .WithSimpleSchedule(
    //                             schedule =>
    //                                 schedule.WithIntervalInSeconds(100)
    //                                     .RepeatForever()));

    //         configure.AddJob<OrderExpirationJob>(opts => opts.StoreDurably());

    //     });

    //     services.AddQuartzHostedService(options =>
    //         {
    //             options.WaitForJobsToComplete = true;
    //         });

    //     services.AddQuartzHostedService();

    //     services.AddScoped<IOrderExpirationScheduler, OrderExpirationScheduler>();

    //     return services;
    // }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
{
    services.AddDbContext<FeedbackDbContext>((sp, options) =>
    {
        // 1. Resolve the interceptor from the service provider
        var outboxInterceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
        var auditInterceptor = sp.GetRequiredService<UpdateAuditableEntitiesInterceptor>();

        // 2. Attach them to the DbContext
        options.UseSqlServer(configuration.GetConnectionString("FeedbackConnection"),
            sqlOptions => sqlOptions.EnableRetryOnFailure())
            .AddInterceptors(outboxInterceptor, auditInterceptor) // THIS IS THE MISSING PART
            .LogTo(Console.WriteLine, LogLevel.Information);
    });

    services.AddScoped<Microsoft.EntityFrameworkCore.DbContext>(provider =>
            provider.GetRequiredService<FeedbackDbContext>());

    return services;
}


    public static IServiceCollection AddCachingService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "FeedbackService";
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
            x.AddConsumer<CustomerNameChangedConsumer>();
            x.AddConsumer<ProductNameChangedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("feedback-data-sync", e =>
                        {
                            e.ConfigureConsumer<CustomerNameChangedConsumer>(context);
                            e.ConfigureConsumer<ProductNameChangedConsumer>(context);
                        });

            });
        });
        return services;
    }


}
