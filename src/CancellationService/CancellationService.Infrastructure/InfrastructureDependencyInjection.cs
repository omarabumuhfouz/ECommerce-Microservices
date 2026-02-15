using CancellationService.Domain.Constants;
using CancellationService.Infrastructure.BackgroundJobs;
using CancellationService.Infrastructure.Data;
using CancellationService.Infrastructure.Interceptors;
using CancellationService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using SharedKernel;
using SharedKernel.Abstractions.Messaging;

namespace CancellationService.Infrastructure
{
    
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
            return services
                    .AddBusinessServices()
                    .AddQuartzInfrastructure()
                    .AddDatabase(configuration)
                    .AddCachingService(configuration)
                    .AddOrderGrpcClient(configuration);
    }


    private static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        // // Change this line in AddBusinessServices:
        // services.Scan(selector => selector
        //     .FromAssemblies(typeof(CancellationService.Application.AssemblyReference).Assembly) // Use your Application project's AssemblyReference
        //     .AddClasses(filter => filter.AssignableTo(typeof(IDomainEventHandler<>)))
        //     .AsImplementedInterfaces()
        //     .WithTransientLifetime());

        services.AddScoped<IIdempotencyService, IdempotencyService>();
        services.AddScoped<IOrderService, OrderGrpcService>();
services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();


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
    services.AddDbContext<CancellationDbContext>((sp, options) =>
    {
        // 1. Resolve the interceptor from the service provider
        var outboxInterceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
        var auditInterceptor = sp.GetRequiredService<UpdateAuditableEntitiesInterceptor>();

        // 2. Attach them to the DbContext
        options.UseSqlServer(configuration.GetConnectionString("CancellationConnection"),
            sqlOptions => sqlOptions.EnableRetryOnFailure())
            .AddInterceptors(outboxInterceptor, auditInterceptor) // THIS IS THE MISSING PART
            .LogTo(Console.WriteLine, LogLevel.Information);
    });

    services.AddScoped<Microsoft.EntityFrameworkCore.DbContext>(provider =>
            provider.GetRequiredService<CancellationDbContext>());

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

}
