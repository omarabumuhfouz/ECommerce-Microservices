using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ProductService.Api.OpenApi.Transformers;
using ProductService.Infrastructure;
using Serilog;
using SharedKernel;
using MassTransit;
using ProductService.Api.Consumers;
using RabbitMQ.Client;
namespace ProductService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
                .AddProductApplicationServices(configuration)
                .AddProductInfrastructureServices(configuration)
                .AddSharedKernelServices(configuration)
                .AddApiDocumentation()
                .AddGrpcService()
                .AddHealthChecksService(configuration)
                .AddOpenTelemetryDistributedTracing()
                .AddMassTransitService();
                

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

 private static IServiceCollection AddGrpcService(this IServiceCollection services)
    {
        services.AddGrpcReflection()
                        .AddGrpc();

        return services;
    }   

 private static IServiceCollection AddHealthChecksService(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: configuration.GetConnectionString("ProductConnection")!,
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


        return services;
    }

    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, loggerCongiguration) =>
            loggerCongiguration.ReadFrom.Configuration(context.Configuration));

        return builder;
    }

    private static IServiceCollection AddOpenTelemetryDistributedTracing(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
    .ConfigureResource(res => res.AddService("productservice"))
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
            x.AddConsumer<DecreaseInventoryConsumer>();
            x.AddConsumer<IncreaseInventoryConsumer>();
            x.AddConsumer<AdjustStockDeltaConsumer>();


            // 3. Configure RabbitMQ
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ReceiveEndpoint("inventory-changes-queue", e =>
                {
                    // Bind the consumers you added earlier
                    e.ConfigureConsumer<DecreaseInventoryConsumer>(context);
                    e.ConfigureConsumer<IncreaseInventoryConsumer>(context);
                    e.ConfigureConsumer<AdjustStockDeltaConsumer>(context);


                    e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                });

            });
        });
        return services;
    }


 

}
