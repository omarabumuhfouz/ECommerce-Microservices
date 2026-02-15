using CartService.Consumers;
using MassTransit;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Client;
using Serilog;
using SharedKernel;
using ShoppingCartService.Api.OpenApi.Transformers;
using ShoppingCartService.Application;
using ShoppingCartService.Infrastructure;

namespace ShoppingCartService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
                .AddApiDocumentation()
                .AddCartsApplicationServices(configuration)
                .AddCartsInfrastructureServices(configuration)
                .AddSharedKernelServices(configuration)
                .AddMassTransitService(configuration)
                .AddHealthChecksService(configuration)
                .AddOpenTelemetryDistributedTracing();


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
   

    private static IServiceCollection AddMassTransitService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<MarkCartAsCheckedOutConsumer>();
            busConfigurator.AddConsumer<RestoreCartConsumer>();

            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username(configuration["MessageBroker:Username"]!);
                    h.Password(configuration["MessageBroker:Password"]!);
                });

                cfg.ReceiveEndpoint("cart-service-checkout-queue", endpoint =>
                {
                    endpoint.ConfigureConsumer<MarkCartAsCheckedOutConsumer>(context);
                    endpoint.ConfigureConsumer<RestoreCartConsumer>(context);
                });
            });
        });


        return services;
    }

    private static IServiceCollection AddHealthChecksService(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitConnectionString =
                        $"amqp://{configuration["MessageBroker:Username"]}:{configuration["MessageBroker:Password"]}@{configuration["MessageBroker:Host"]}/";

        services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: configuration.GetConnectionString("CartConnection")!,
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
        // Now you can access 'builder.Host'
        builder.Host.UseSerilog((context, loggerCongiguration) =>
            loggerCongiguration.ReadFrom.Configuration(context.Configuration));

        return builder;
    }

    private static IServiceCollection AddOpenTelemetryDistributedTracing(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
                .ConfigureResource(res => res.AddService("cartservice"))
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



}
