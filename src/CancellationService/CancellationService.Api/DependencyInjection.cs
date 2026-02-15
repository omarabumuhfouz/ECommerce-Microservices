using CancellationService.Api;
using CancellationService.Api.Transformers;
using CancellationService.Application;
using CancellationService.Infrastructure;
using MassTransit;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Client;
using Serilog;
using SharedKernel;

namespace CancellationService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddCancellationServiceApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
                .AddSharedKernelServices(configuration)
                .AddInfrastructureServices(configuration)
                .AddApplicationServices()
                .AddApiDocumentation()
                .AddGrpcService()
                .AddHealthChecksService(configuration)
                .AddOpenTelemetryDistributedTracing()
                .AddMassTransitService()
                .AddHttpContextAccessor();

        // .AddOutputCacheService();



        return services;
    }

    private static IServiceCollection AddGrpcService(this IServiceCollection services)
    {
        services.AddGrpcReflection()
                        .AddGrpc();

        return services;
    }

    private static IServiceCollection AddApiDocumentation(this IServiceCollection services)
    {

        services.AddOpenApi("v1", options =>
        {
            // Use transformer class
            options.AddDocumentTransformer<DefaultInfoTransformer>();


            // Security Scheme config
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            options.AddOperationTransformer<BearerSecuritySchemeTransformer>();
        });

        return services;
    }

    private static IServiceCollection AddHealthChecksService(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitConnectionString =
                        $"amqp://{configuration["MessageBroker:Username"]}:{configuration["MessageBroker:Password"]}@{configuration["MessageBroker:Host"]}/";

        services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: configuration.GetConnectionString("CancellationConnection")!,
                    name: "SQL Server",
                    tags: new[] { "database", "critical" })

                .AddRedis(
                    redisConnectionString: configuration.GetConnectionString("Redis")!,
                    name: "Redis Cache",
                    tags: new[] { "cache", "critical" })

                .AddRabbitMQ(async sp =>
{
    var factory = new ConnectionFactory
    {
        HostName = configuration["MessageBroker:Host"]!, 
        UserName = configuration["MessageBroker:Username"]!,
        Password = configuration["MessageBroker:Password"]!,
        Port = 5672 
    };
    return await factory.CreateConnectionAsync();
},
name: "RabbitMQ",
tags: new[] { "messagebroker", "critical" });

        // // 2. Add the Health Checks UI
        // services.AddHealthChecksUI(options =>
        // {
        //     // Tell the UI to poll your "/health/ready" endpoint
        //     options.AddHealthCheckEndpoint("Customer Service", "http://localhost:7001/health/ready");

        //     // How often the UI should poll (e.g., every 5 seconds)
        //     options.SetEvaluationTimeInSeconds(5);

        // })
        // .AddInMemoryStorage();// Use in-memory

        return services;
    }

    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        // Now you can access 'builder.Host'
        builder.Host.UseSerilog((context, loggerCongiguration) =>
            loggerCongiguration.ReadFrom.Configuration(context.Configuration));

        return builder;
    }

    private static IServiceCollection AddOpenTelemetryDistributedTracing(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
                .ConfigureResource(res => res.AddService("cancellationservice"))
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation().
        AddHttpClientInstrumentation();

        tracing.AddOtlpExporter();
    })

    // Add Metrics 
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation().
        AddHttpClientInstrumentation();

        metrics.AddOtlpExporter().
        AddPrometheusExporter(); // Prometheus and Grafana
    });



        return services;
    }

    private static IServiceCollection AddMassTransitService(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            // 1. Register the Consumer
            // x.AddConsumer<UserRegisteredConsumer>();

            // 3. Configure RabbitMQ
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                // // 4. Define the Queue Name
                // // This creates a queue named "customer-create-profile"
                // cfg.ReceiveEndpoint("queue-name-here", e =>
                // {
                //     // e.ConfigureConsumer<UserRegisteredConsumer>(context);

                //     // Tip: If creating a customer is critical, configure retries here
                //     e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                // });
            });
        });
        return services;
    }


}
